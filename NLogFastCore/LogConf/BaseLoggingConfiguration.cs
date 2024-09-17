using NLog;
using NLog.Config;
using NLog.Targets;

using NLogFastCore.Builders.Targets;

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
        /// <param name="targetBuilder">Target-Конструктор </param>
        protected BaseLoggingConfiguration(ITargetBuilder targetBuilder)
        {
            _minLogLevel = LogLevel.Warn;
            _maxLogLevel = LogLevel.Fatal;
            TargetBuilder = targetBuilder;
            if (targetBuilder is RabbitMQTargetBuilder rabbitMQTargetBuilder)
            {
                rabbitMQTargetBuilder.Topic = TopicForRabbitMQ;
            }
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
        /// Target-Конструктор 
        /// </summary>
        public ITargetBuilder TargetBuilder { get; set; }

        /// <summary>
        /// Получение таргетов
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Target> GetTargets();

        /// <summary>
        /// Установка прав доступа
        /// </summary>
        /// <param name="loggingConfiguration">Конфигурация</param>
        /// <param name="targets">Таргеты</param>
        protected abstract void SetLoggingRules(LoggingConfiguration loggingConfiguration, IEnumerable<Target> targets);

        ///<inheritdoc/>
        public void SetLogLevel(LogLevel minLogLevel, LogLevel maxLogLevel)
        {
            _minLogLevel = minLogLevel;
            _maxLogLevel = maxLogLevel;
        }

        /// <inheritdoc/>
        public ISetupBuilder Setup(ISetupBuilder setupBuilder)
        {
            return setupBuilder.LoadConfiguration(x =>
            {
                var config = x.Configuration ?? new LoggingConfiguration();
                var targets = GetTargets();
                foreach (var target in targets)
                {
                    config.AddTarget(target);
                }
                SetLoggingRules(config, targets);
            });
        }

        /// <summary>
        /// Название Routing Key для очереди RabbitMQ
        /// </summary>
        protected abstract string TopicForRabbitMQ { get; }
    }
}
