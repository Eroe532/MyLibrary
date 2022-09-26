using NLog;
using NLog.Config;
using NLog.Targets;

using NLogFastCore.Enums;
using NLogFastCore.Provider;

namespace NLogFastCore.LogConf
{
    /// <summary>
    /// Базовая лог конфигурация
    /// </summary>
    public abstract class BaseLoggingConfiguration : ILoggingConfiguration
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        protected BaseLoggingConfiguration()
        {
            _minLogLevel = LogLevel.Warn;
            _maxLogLevel = LogLevel.Fatal;
        }

        /// <summary>
        /// Минимальный уровень логгирования
        /// </summary>
        protected LogLevel _minLogLevel;

        /// <summary>
        /// Максимальный уровень логгирования
        /// </summary>
        protected LogLevel _maxLogLevel;

        /// <summary>
        /// Получение таргетов
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="dbProviderSpecificFunctionality">Пройвайдер БД</param>
        /// <returns></returns>
        protected abstract IEnumerable<Target> GetTargets(string connectionString, IDbProviderSpecificFunctionality dbProviderSpecificFunctionality);

        /// <summary>
        /// Установка пров доступа
        /// </summary>
        /// <param name="loggingConfiguration">Конфигурация</param>
        /// <param name="targets">Таргкты</param>
        protected abstract void SetLoggingRules(LoggingConfiguration loggingConfiguration, IEnumerable<Target> targets);

        /// <summary>
        /// Выполнение настроек логирования
        /// </summary>
        /// <param name="setupBuilder"></param>
        /// <param name="connectionString"> Строка подключения к базе данных </param>
        /// <param name="dbProviderSpecificFunctionality"> Тип провайдера работы с базой данных </param>
        public ISetupBuilder Setup(ISetupBuilder setupBuilder, string connectionString, IDbProviderSpecificFunctionality dbProviderSpecificFunctionality)
        {
            return setupBuilder.LoadConfiguration(x =>
            {
                var config = x.Configuration ?? new LoggingConfiguration();
                var targets = GetTargets(connectionString, dbProviderSpecificFunctionality);
                foreach (var target in targets)
                {
                    config.AddTarget(target);
                }
                SetLoggingRules(config, targets);
            });
        }

        /// <inheritdoc/>
        public ISetupBuilder Setup(ISetupBuilder setupBuilder, string connectionString, DbProviderType dbProviderType)
            => Setup(setupBuilder, connectionString, GetDbProviderSpecificFunctionality(dbProviderType));

        private static IDbProviderSpecificFunctionality GetDbProviderSpecificFunctionality(DbProviderType dbProviderType)
        {
            return dbProviderType switch
            {
                DbProviderType.MicrosoftSQLServer => new MicrosoftSQLServerProvider(),
                DbProviderType.PostgreSQL => new NpgsqlProvider(),
                _ => throw new NotImplementedException(),
            };
        }

        ///<inheritdoc/>
        public void SetLogLevel(LogLevel minLogLevel, LogLevel maxLogLevel)
        {
            _minLogLevel = minLogLevel;
            _maxLogLevel = maxLogLevel;
        }
    }
}
