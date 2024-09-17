using ConfigurationManager;

using Microsoft.Extensions.Configuration;

using NLog;

using NLogFastCore.Builders.Targets;
using NLogFastCore.Enums;
using NLogFastCore.LogConf.ConnectionString;

namespace NLogFastCore.LogRegistrar
{
    /// <summary>
    /// Регистратор логгера
    /// </summary>
    public class LoggerRabbitMQ
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
        public RabbitMQConnection Connection { get; private set; } = new();

        /// <summary>
        /// Добавить логгер для регистрации из списка доступных
        /// </summary>
        /// <param name="loggerRegistrar">Регистратор логгера</param>
        /// <param name="loggerType"> Тип добавляемого логгера </param>
        public LoggerRabbitMQ(LoggerRegistrar loggerRegistrar, LoggerType loggerType)
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
        public LoggerRabbitMQ SetLevel(LogLevel? minLogLevel = null, LogLevel? maxLogLevel = null)
        {
            _minLogLevel = minLogLevel;
            _maxLogLevel = maxLogLevel;
            return this;
        }

        /// <summary>
        /// Установить подключение для настройки логгеров
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public LoggerRabbitMQ SetConnectionString(RabbitMQConnection connection)
        {
            Connection = connection;
            return this;
        }

        /// <summary>
        /// Установить строку подключения для настройки логгеров по названию
        /// </summary>
        /// <param name="connectionString"> Название строки подключения </param>
        /// <param name="configuration">Конфигурация</param>
        /// <returns></returns>
        public LoggerRabbitMQ SetConnectionStringByName(string connectionString, IConfiguration? configuration = null)
        {
            configuration ??= ConfigManager.GetIConfiguration();
            LoggerRegistrar.Check(connectionString, "Название строки подключения пусто");
            Connection = new RabbitMQConnection(configuration.GetConnectionString(connectionString)!);
            return this;
        }


        /// <summary>
        /// Установить строку подключения для настройки логгеров по названию
        /// </summary>
        /// <param name="str"> Cтрокf подключения </param>
        /// <returns></returns>
        public LoggerRabbitMQ SetConnectionStringByString(string str)
        {
            Connection = new RabbitMQConnection(str);
            return this;
        }

        /// <summary>
        /// Настроить добавленные логгеры
        /// </summary>
        public LoggerRegistrar Save(string targetName = "RabbitMQTarget")
        {
            var targetBuilder = new RabbitMQTargetBuilder(targetName, Connection);
            var loggingConfiguration = LoggerRegistrar.GetLoggingConfiguration(targetBuilder, _loggerType);
            loggingConfiguration.SetLogLevel(_minLogLevel ??= LogLevel.Warn, _maxLogLevel ??= LogLevel.Fatal);
            _loggerRegistrar.Add(loggingConfiguration);
            return _loggerRegistrar;
        }


    }
}
