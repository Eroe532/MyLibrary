using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace ConfigurationManager;
/// <summary>
/// Конфигурация
/// </summary>
public static class ConfigManager
{
    /// <summary>
    /// Конфигурация
    /// </summary>
    private static IConfiguration? _configuration;

    /// <summary>
    /// Все файлы конфигураций
    /// </summary>
    private static HashSet<string> _fileName = new();

    private static string[] _arg = Array.Empty<string>();

    private static ConfigurationBuilder? _builder;

    /// <summary>
    /// Получить IConfiguration
    /// </summary>
    /// <returns></returns>
    public static IConfiguration GetIConfiguration(params string[] jsonFileName)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var addFile = jsonFileName.Any(file => _fileName.Add(file));
        if (addFile || _configuration is null)
        {
            var configurationBuilder = _builder ?? new ConfigurationBuilder();
            configurationBuilder
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            foreach (var file in jsonFileName)
            {
                configurationBuilder
                  .AddJsonFile($"{file}.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"{file}.{environment}.json", optional: true, reloadOnChange: true);
            }
            if (environment == "Development")
            {
                configurationBuilder.AddUserSecrets(Assembly.GetEntryAssembly()!, true);
            }
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddCommandLine(_arg);
            _configuration = configurationBuilder.Build();
        }
        return _configuration;
    }


    /// <summary>
    /// Получить IConfiguration
    /// </summary>
    /// <returns></returns>
    public static IConfiguration GetIConfiguration(string[] args, params string[] jsonFileName)
    {
        _arg = args;
        return GetIConfiguration(jsonFileName);
    }
}
