using CaTRepository.Factory;

using FileStorage.Api.BLL;
using FileStorage.DAL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.Api;

/// <summary>
/// Методы расширения для отображения данных о превышении максимума файла
/// </summary>
public static class FileStorageExeption
{
    /// <summary>
    /// Вывод максимального размера файла в понятном виде
    /// </summary>
    /// <param name="value">Величина</param>
    /// <returns></returns>
    public static string GetSizeInBytesDescription(this long value)
    {
        return value < 1024
            ? GetDeclined(value, @"Байт")
            : value < 1048576
                ? GetDeclined(value, 1024, @"Килобайт")
                : value < 1073741824
                    ? GetDeclined(value, 1048576, @"Мегабайт")
                    : GetDeclined(value, 1073741824, @"Гигабайт");
    }

    private static string GetDeclined(long value, long divider, string singularUnit)
    {
        var resultValue = Math.Round(value / (double)divider, 2);
        var integerPart = Math.Truncate(resultValue);
        var fractionalPart = resultValue - integerPart;
        return fractionalPart != 0d
            ? $@"{resultValue} {singularUnit}а"
            : GetDeclined((long)integerPart, singularUnit);
    }

    private static string GetDeclined(long value, string singularUnit)
    {
        var remainder = value % 10;
        return $@"{value} {(remainder is >= 2 and <= 4 ? singularUnit + @"а" : singularUnit)}";
    }

    /// <summary>
    /// Зарегистрировать файловое хранилище
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="connectionString">Строка подключения</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns></returns>
    public static IServiceCollection AddFileStorage<TContext>(this IServiceCollection services, string connectionString = "FileStorageProvider", IConfiguration? configuration = null) where TContext : DbContext
    {
        configuration ??= CaTConfiguration.CaTConfig.GetIConfiguration();
        var maxFileSize = configuration.GetValue<long>("MaximumFileSize");
        if (maxFileSize == 0)
        {
            maxFileSize = 52428800L;
        }
        var maximumFileSize = new MaximumFileSize(maxFileSize);
        services.AddSingleton(maximumFileSize);
        services.AddRepositoryFactory<FileRepository<TContext>, TContext>();

        var sqlConnectionStringToData = configuration.GetConnectionString(connectionString) ?? connectionString;
        services.AddDbContextFactory<FileStorageContext>(options => options.UseNpgsql(sqlConnectionStringToData));
        services.AddRepositoryFactory<FileRepository<FileStorageContext>, FileStorageContext>();

        services.AddTransient<FileLogic<TContext>>();
        return services;
    }

    /// <summary>
    /// Зарегистрировать файловое хранилище
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="connectionString">Строка подключения</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns></returns>
    public static IServiceCollection AddFileStorage(this IServiceCollection services, string connectionString = "FileStorageProvider", IConfiguration? configuration = null)
    {
        return services.AddFileStorage<FileStorageContext>(connectionString, configuration);
    }
}
