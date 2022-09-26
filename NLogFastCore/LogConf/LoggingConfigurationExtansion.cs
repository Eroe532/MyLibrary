
using NLog.Config;

using NLogFastCore.Enums;
using NLogFastCore.LogConf;

namespace NLogFastCore.LogConf
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class LoggingConfigurationExtansion
    {
        /// <summary>
        /// Выполнение настроек логирования
        /// </summary>
        /// <param name="setupBuilder"></param>
        /// <param name="loggingConfiguration">Конфигурация логера</param>
        /// <param name="connectionString"> Строка подключения к базе данных </param>
        /// <param name="dbProviderType"> Тип провайдера работы с базой данных </param>
        public static ISetupBuilder Setup(this ISetupBuilder setupBuilder, ILoggingConfiguration loggingConfiguration, string connectionString, DbProviderType dbProviderType)
        {
            return loggingConfiguration.Setup(setupBuilder, connectionString, dbProviderType);
        }
    }
}
