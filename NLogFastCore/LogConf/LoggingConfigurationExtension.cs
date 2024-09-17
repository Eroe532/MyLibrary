using NLog.Config;

namespace NLogFastCore.LogConf
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class LoggingConfigurationExtension
    {
        /// <summary>
        /// Выполнение настроек логирования
        /// </summary>
        /// <param name="setupBuilder"></param>
        /// <param name="loggingConfiguration">Конфигурация логгера</param>
        public static ISetupBuilder SetupExt(this ISetupBuilder setupBuilder, ILoggingConfiguration loggingConfiguration)
        {
            return loggingConfiguration.Setup(setupBuilder);
        }
    }
}
