
using CaTConfiguration;

using CaTLogging.LogRegistrar;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using NLog.Web;

using FileStorage.Api;
using CaTSwagger;
using CaTSecurity;
using NLog;
using FileStorage.Test.API;
using System.Reflection;

var configuration = CaTConfig.GetIConfiguration(args, Array.Empty<string>());
var logger = LoggerRegistrar.Setup().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddConfiguration(configuration);

    #region Logger
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion
    builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

    builder.Services.AddFileStorage();

    #region
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession();

    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddCors();

    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueCountLimit = int.MaxValue;
    });
    #endregion
    #region Swagger
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddSwaggerCaT(builder.Environment);
    }
    #endregion

    #region Secure
    builder.Services.AddSecureJWT<FilePolicy>(builder.Environment);
    #endregion
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        foreach (var dsfsdf in
        builder.Configuration.GetSection("Kestrel:EndPoints").GetChildren())
        {
            logger.Fatal(dsfsdf.GetValue<string>("Url"));
        }

        app.UseSwaggerCaT("contractbackend_swagg");
        app.UseDeveloperExceptionPage();
        Console.WriteLine(app.Environment.EnvironmentName);
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseRouting();
    app.UseAuthorization();

    app.UseSession();
    app.UseCors(builder => builder.WithOrigins(app.Configuration.GetSection("Cors")
                .GetChildren().Select(m => m.Value).ToArray()!)
            .AllowAnyMethod()
            .AllowAnyHeader());
    app.MapControllers();
    app.Run();
}
catch (Exception exception)
{
    // NLog: catch buildup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
