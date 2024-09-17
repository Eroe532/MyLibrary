using ConfigurationManager;

using Microsoft.Extensions.Configuration;

using NLog;
using NLog.Config;
using NLog.Web;

using NLogFastCore.Builders.Targets;
using NLogFastCore.Enums;
using NLogFastCore.Layouts;
using NLogFastCore.LogConf;
using NLogFastCore.LogConf.Configurations;

namespace NLogFastCore.LogRegistrar;

/// <summary>
/// Регистратор логгера
/// </summary>
public class LoggerRegistrar
{
    private readonly HashSet<ILoggingConfiguration> _loggingConfigurations = new();

    /// <summary>
    /// InternalLogging
    /// </summary>
    private bool _setupInternalLogging;

    /// <summary>
    /// Добавить логгер для регистрации из списка доступных
    /// </summary>
    /// <param name="loggerType"> Тип добавляемого логгера </param>
    public LoggerDataBase AddLogToDataBase(LoggerType loggerType)
    {
        return new LoggerDataBase(this, loggerType);
    }

    /// <summary>
    /// Добавить логгер для регистрации из списка доступных
    /// </summary>
    /// <param name="loggerType"> Тип добавляемого логгера </param>
    public LoggerRabbitMQ AddLogToRabbitMQ(LoggerType loggerType)
    {
        return new LoggerRabbitMQ(this, loggerType);
    }

    /// <summary>
    /// Добавить логгер
    /// </summary>
    /// <param name="loggingConfigurations"></param>
    /// <returns></returns>
    public LoggerRegistrar Add(params ILoggingConfiguration[] loggingConfigurations)
    {
        foreach (var loggingConfiguration in loggingConfigurations)
        {
            _loggingConfigurations.Add(loggingConfiguration);
        }

        return this;
    }

    /// <summary>
    /// Установка Internal
    /// </summary>
    /// <returns></returns>
    public LoggerRegistrar AddInternal()
    {
        _setupInternalLogging = true;
        return this;
    }

    /// <summary>
    /// Настроить добавленные логгеры
    /// </summary>
    public ISetupBuilder Setup()
    {
        if (_setupInternalLogging)
        {
            InternalLoggingConfiguration.Setup();
        }
        var setupBuilder = LogManager.Setup().RegisterLayouts().RegisterNLogWeb();
        foreach (var loggingConfiguration in _loggingConfigurations)
        {
            setupBuilder.SetupExt(loggingConfiguration);
        }
        return setupBuilder;
    }

    /// <summary>
    /// Проверить строку на пустоту
    /// </summary>
    internal static void Check(string stringToCheck, string formatMessage, params object[] formatValues)
    {
        if (string.IsNullOrWhiteSpace(stringToCheck))
        {
            throw new ArgumentException(string.Format(formatMessage, formatValues));
        }
    }

    /// <summary>
    /// Получения логгера по типу
    /// </summary>
    /// <param name="targetBuilder">Target-Конструктор</param>
    /// <param name="loggerType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static ILoggingConfiguration GetLoggingConfiguration(ITargetBuilder targetBuilder, LoggerType loggerType)
        => loggerType switch
        {
            //LoggerType.MethodLogger => new MethodLoggingConfiguration(),
            LoggerType.ErrorLogger => new ErrorLoggingConfiguration(targetBuilder),
            _ => throw new ArgumentException("Неизвестный тип логгера")
        };

    /// <summary>
    /// Загрузка настроек из конфигурации
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static ISetupBuilder Setup(IConfiguration? configuration = null, string? sectionName = null)
    {
        configuration ??= ConfigManager.GetIConfiguration();
        var loggerRegistrar = new LoggerRegistrar();

        var configurationSection = configuration.GetSection(sectionName ?? "CaTLogging");

        var notInternal = configurationSection.GetValue<bool>("NotInternal");
        if (!notInternal)
        {
            loggerRegistrar = loggerRegistrar.AddInternal();
        }
        var minLevelTotal = ParseLogLevel(configurationSection.GetValue<string>("MinimumLevel") ?? configurationSection.GetValue<string>("MinLevel"));
        var maxLevelTotal = ParseLogLevel(configurationSection.GetValue<string>("MaximumLevel") ?? configurationSection.GetValue<string>("MaxLevel"));

        var arrayOfWriteTo = configurationSection.GetSection("WriteTo").GetChildren();
        if (arrayOfWriteTo.Any())
        {
            foreach (var element in arrayOfWriteTo)
            {
                var name = element.GetValue<string>("Name");
                if (name == null)
                {
                    continue;
                }
                var logType = LoggerType.ErrorLogger;
                var minLevel = ParseLogLevel(configurationSection.GetValue<string>("MinimumLevel") ?? configurationSection.GetValue<string>("MinLevel")) ?? minLevelTotal ?? LogLevel.Warn;
                var maxLevel = ParseLogLevel(configurationSection.GetValue<string>("MaximumLevel") ?? configurationSection.GetValue<string>("MaxLevel")) ?? maxLevelTotal ?? LogLevel.Fatal;
                switch (name)
                {
                    case "RabbitMQ":
                        {
                            loggerRegistrar = loggerRegistrar.AddLogToRabbitMQ(logType)
                                .SetConnectionStringByString(element.GetValue<string>("ConnectionString") ?? string.Empty)
                                .SetLevel(minLevel, maxLevel)
                                .Save();
                            break;
                        }
                    case "PostgreSQL":
                        {
                            loggerRegistrar = loggerRegistrar.AddLogToDataBase(logType)
                                .SetDbProvider(DbProviderType.PostgreSQL)
                                .SetConnectionString(element.GetValue<string>("ConnectionString") ?? string.Empty)
                                .SetLevel(minLevel, maxLevel)
                                .Save();
                            break;
                        }
                    case "MSSqlServer":
                        {
                            loggerRegistrar = loggerRegistrar.AddLogToDataBase(logType)
                                .SetDbProvider(DbProviderType.MicrosoftSQLServer)
                                .SetConnectionString(element.GetValue<string>("ConnectionString") ?? string.Empty)
                                .SetLevel(minLevel, maxLevel)
                                .Save();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        return loggerRegistrar.Setup();
    }

    private static LogLevel? ParseLogLevel(string? loglevel)
    {
        return loglevel is not null ? LogLevel.FromString(loglevel) : null;
    }
}


