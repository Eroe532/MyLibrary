using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskSchedulerCore
{
    /// <summary>
    /// Фонавая задача работающая по времени (На основе таймера)
    /// </summary>
    public abstract class TaskSchedulerHostedService : IHostedService, IDisposable
    {
        /// <summary>
        /// Логирование
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Список таймеров
        /// </summary>
        private List<Timer> _timers = new();

        /// <summary>
        /// Фонавая задача работающая по времени
        /// </summary>
        public TaskSchedulerHostedService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Запуск
        /// </summary>
        /// <param name="stoppingToken">Токен отмены</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            StartAction();
            _logger.LogInformation("Timed Hosted Service running.");
            _timers.AddRange(ScheduleTasks());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Добавление задач
        /// </summary>
        protected abstract IEnumerable<Timer> ScheduleTasks();

        /// <summary>
        /// Добавление задач
        /// </summary>
        protected abstract Task StartAction();

        /// <summary>
        /// Остановка
        /// </summary>
        /// <param name="stoppingToken">Токен отмены</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timers.ForEach(timer => timer.Change(Timeout.Infinite, 0));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timers.Any())
                {
                    _timers.ForEach(timer => timer.Dispose());
                    _timers = new();
                }
            }
        }
    }
}