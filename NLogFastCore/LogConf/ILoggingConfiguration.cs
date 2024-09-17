using NLog;
using NLog.Config;

using NLogFastCore.Builders.Targets;

namespace NLogFastCore.LogConf
{
    /// <summary>
    /// Интерфейс конфигурации логгера
    /// </summary>
    public interface ILoggingConfiguration
    {
        /// <summary>
        /// Target-Конструктор
        /// </summary>
        public ITargetBuilder TargetBuilder { get; set; }

        /// <summary>
        /// Выполнение настроек логирования
        /// </summary>
        /// <param name="setupBuilder"></param>
        ISetupBuilder Setup(ISetupBuilder setupBuilder);

        /// <summary>
        /// Установка уровней логупрования
        /// </summary>
        /// <param name="minLogLevel">Минимальный</param>
        /// <param name="maxLogLevel">Максимальный</param>
        void SetLogLevel(LogLevel minLogLevel, LogLevel maxLogLevel);
    }

}
