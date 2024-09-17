using NLog;

using NLogFastCore.Builders.Targets;
using NLogFastCore.DBProvider;
using NLogFastCore.Enums;

namespace NLogFastCore.LogRegistrar
{
    /// <summary>
    /// Регистратор логгера
    /// </summary>
    public class LoggerDataBase
    {
        /// <summary>
        /// Регистратор логгера
        /// </summary>
        private readonly LoggerRegistrar _loggerRegistrar;

        /// <summary>
        /// Тип логгера
        /// </summary>
        private readonly LoggerType _loggerType;

        /// <summary>
        /// Минимальный уровень логгирования
        /// </summary>
        private LogLevel? _minLogLevel = null;
        /// <summary>
        /// Максимальный уровень логгирования
        /// </summary>
        private LogLevel? _maxLogLevel = null;

        /// <summary>
        /// Строка подключения
        /// </summary>
        private string _connectionString = "${configsetting:item=ConnectionStrings.LogSqlConnection}";

        /// <summary>
        /// Провайдер БД
        /// </summary>
        private DbProviderType _dbProviderType;

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString
        {
            get => _connectionString;
            private set
            {
                LoggerRegistrar.Check(value, "Строка подключения пуста");
                _connectionString = value;
            }
        }

        /// <summary>
        /// Добавить логгер для регистрации из списка доступных
        /// </summary>
        /// <param name="loggerRegistrar">Регистратор логгера</param>
        /// <param name="loggerType"> Тип добавляемого логгера </param>
        public LoggerDataBase(LoggerRegistrar loggerRegistrar, LoggerType loggerType)
        {
            _loggerType = loggerType;
            _loggerRegistrar = loggerRegistrar;
        }

        /// <summary>
        /// Добавить логгер для регистрации из списка доступных
        /// </summary>
        /// <param name="minLogLevel">Минимальный уровень логгирования
        /// по умолчанию warn</param>
        /// <param name="maxLogLevel">Максимум уровень логгирования
        /// по умолчанию fatal</param>
        public LoggerDataBase SetLevel(LogLevel? minLogLevel = null, LogLevel? maxLogLevel = null)
        {
            _minLogLevel = minLogLevel;
            _maxLogLevel = maxLogLevel;
            return this;
        }

        /// <summary>
        /// Установить строку подключения для настройки логгеров
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public LoggerDataBase SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }

        /// <summary>
        /// Установить строку подключения для настройки логгеров по названию
        /// </summary>
        /// <param name="connectionString"> Название строки подключения </param>
        /// <returns></returns>
        public LoggerDataBase SetConnectionStringByName(string connectionString)
        {
            LoggerRegistrar.Check(connectionString, "Название строки подключения пусто");
            ConnectionString = "${configsetting:item=ConnectionStrings." + connectionString + "}";
            return this;
        }

        /// <summary>
        /// Установить провайдер баз данных для настройки логгеров
        /// </summary>
        /// <param name="dbProviderType"> Тип провайдера баз данныых </param>
        /// <returns></returns>
        public LoggerDataBase SetDbProvider(DbProviderType dbProviderType)
        {
            _dbProviderType = dbProviderType;
            return this;
        }

        /// <summary>
        /// Настроить добавленные логгеры
        /// </summary>
        public LoggerRegistrar Save(string targetName = "DatabaseTarget", string tableName = "ErrorLoggers")
        {
            var targetBuilder = new DatabaseTargetBuilder(targetName, GetDbProviderSpecificFunctionality(_dbProviderType), _connectionString, tableName);
            var loggingConfiguration = LoggerRegistrar.GetLoggingConfiguration(targetBuilder, _loggerType);
            loggingConfiguration.SetLogLevel(_minLogLevel ??= LogLevel.Warn, _maxLogLevel ??= LogLevel.Fatal);
            _loggerRegistrar.Add(loggingConfiguration);
            return _loggerRegistrar;
        }


        private static IDbProviderSpecificFunctionality GetDbProviderSpecificFunctionality(DbProviderType dbProviderType)
        {
            return dbProviderType switch
            {
                DbProviderType.MicrosoftSQLServer => new MicrosoftSQLServerProvider(),
                DbProviderType.PostgreSQL => new NpgsqlProvider(),
                _ => throw new NotImplementedException(),
            };
        }


    }
}
