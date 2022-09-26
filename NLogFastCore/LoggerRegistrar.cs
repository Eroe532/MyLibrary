using NLog;
using NLog.Config;
using NLog.Web;

using NLogFastCore.Enums;
using NLogFastCore.Layouts;
using NLogFastCore.LogConf;

namespace NLogFastCore
{
    /// <summary>
    /// Регистротор логгера
    /// </summary>
    public class LoggerRegistrar
    {
        private readonly HashSet<ILoggingConfiguration> _loggingConfigurations = new();

        private string _connectionString = "${configsetting:item=ConnectionStrings.LogSqlConnection}";

        /// <summary>
        /// Провайдер БД
        /// </summary>
        private DbProviderType _dbProviderType;

        /// <summary>
        /// InternalLogging
        /// </summary>
        private bool _setupInternalLogging;

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString
        {
            get => _connectionString;
            private set
            {
                Check(value, "Строка подключения пуста");
                _connectionString = value;
            }
        }

        /// <summary>
        /// Добавить логгер для регистрации из списка доступных
        /// </summary>
        /// <param name="loggerType"> Тип добавляемого логгера </param>
        /// <param name="minLogLevel">Минимальный уровень логгирования
        /// по умолчанию warn</param>
        /// <param name="maxLogLevel">Максимум уровень логгирования
        /// по умолчанию fatal</param>
        public LoggerRegistrar Add(LoggerType loggerType, LogLevel? minLogLevel = null, LogLevel? maxLogLevel = null)
        {
            var loggingConfigurations = GetLoggingConfiguration(loggerType);
            minLogLevel ??= LogLevel.Warn;
            maxLogLevel ??= LogLevel.Fatal;
            loggingConfigurations.SetLogLevel(minLogLevel, maxLogLevel);
            _loggingConfigurations.Add(loggingConfigurations);
            return this;
        }

        /// <summary>
        /// Добавить логгер для регистрации из списка доступных
        /// </summary>
        /// <param name="loggerType"> Тип добавляемого логгера </param>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="minLogLevel">Минимальный уровень логгирования
        /// по умолчанию warn</param>
        /// <param name="maxLogLevel">Максимум уровень логгирования
        /// по умолчанию fatal</param>
        public LoggerRegistrar AddDbLogger(LoggerType loggerType, string tableName, LogLevel? minLogLevel = null, LogLevel? maxLogLevel = null)
        {
            var loggingConfigurations = GetDBLoggingConfiguration(loggerType, tableName);
            minLogLevel ??= LogLevel.Warn;
            maxLogLevel ??= LogLevel.Fatal;
            loggingConfigurations.SetLogLevel(minLogLevel, maxLogLevel);
            _loggingConfigurations.Add(loggingConfigurations);
            return this;
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
        /// Установить строку подключения для настройки логгеров
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public LoggerRegistrar SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }

        /// <summary>
        /// Установить строку подключения для настройки логгеров по названию
        /// </summary>
        /// <param name="connectionString"> Название строки подключения </param>
        /// <returns></returns>
        public LoggerRegistrar SetConnectionStringByName(string connectionString)
        {
            Check(connectionString, "Название строки подключения пусто");
            ConnectionString = "${configsetting:item=ConnectionStrings." + connectionString + "}";
            return this;
        }

        /// <summary>
        /// Установить провайдер баз данных для настройки логгеров
        /// </summary>
        /// <param name="dbProviderType"> Тип провайдера баз данныых </param>
        /// <returns></returns>
        public LoggerRegistrar SetDbProvider(DbProviderType dbProviderType)
        {
            _dbProviderType = dbProviderType;
            return this;
        }

        /// <summary>
        /// Настроить добавленные логгеры
        /// </summary>
        public ISetupBuilder Setup()
        {
            LayoutsRegistration.RegisterLayouts();
            if (_setupInternalLogging)
            {
                InternalLoggingConfiguration.Setup();
            }
            var setupBuilder = LogManager.Setup().RegisterNLogWeb();
            foreach (var loggingConfiguration in _loggingConfigurations)
            {
                setupBuilder.Setup(loggingConfiguration, _connectionString, _dbProviderType);
            }
            return setupBuilder;
        }

        /// <summary>
        /// Проверить строку на пустоту
        /// </summary>
        private static void Check(string stringToCheck, string formatMessage, params object[] formatValues)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentException(string.Format(formatMessage, formatValues));
            }
        }

        /// <summary>
        /// Получения логера по типу
        /// </summary>
        /// <param name="loggerType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static ILoggingConfiguration GetLoggingConfiguration(LoggerType loggerType)
            => loggerType switch
            {
                LoggerType.ErrorLogger => new ErrorLoggingConfiguration(),
                _ => throw new ArgumentException("Неизвестный тип логгера")
            };

        /// <summary>
        /// Получения логера по типу
        /// </summary>
        /// <param name="loggerType"></param>
        /// <param name="tableName">Название таблицы</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static ILoggingConfiguration GetDBLoggingConfiguration(LoggerType loggerType, string tableName)
            => loggerType switch
            {
                LoggerType.ErrorLogger => new ErrorLoggingConfiguration(tableName),
                _ => throw new ArgumentException("Неизвестный тип логгера или логгер не рпсчитан на запись в базу данных")
            };
    }
}
