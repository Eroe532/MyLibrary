using NLog;
using NLog.Config;

using NLogFastCore.Enums;

namespace NLogFastCore.LogConf
{
    /// <summary>
    /// Интерфейс конфигурации логгера
    /// </summary>
    public interface ILoggingConfiguration
    {
        /// <summary>
        /// Выполнение настроек логирования
        /// </summary>
        /// <param name="setupBuilder"></param>
        /// <param name="connectionString"> Строка подключения к базе данных </param>
        /// <param name="dbProviderType"> Тип провайдера работы с базой данных </param>
        ISetupBuilder Setup(ISetupBuilder setupBuilder, string connectionString, DbProviderType dbProviderType);

        /// <summary>
        /// Установка уровней логупрования
        /// </summary>
        /// <param name="minLogLevel">Минимальный</param>
        /// <param name="maxLogLevel">Максимальный</param>
        void SetLogLevel(LogLevel minLogLevel, LogLevel maxLogLevel);
    }

}
