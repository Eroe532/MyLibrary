using EventStore.Client;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventStoreDBLibrary.ESDB
{
    /// <summary>
    /// Класс для слушателя(подписчика на поток) в EventStoreDB Который записывает в бд информацию
    /// </summary>
    /// <typeparam name="TContext">Класс понтекста бд</typeparam>
    public abstract class ESDBSubscriberHostedService<TContext> : ESDBSubscriber<TContext>, IHostedService, IDisposable where TContext : DbContext
    {

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="contextFactory">Фабрика контекстов Бд</param>
        /// <param name="logger">Логгер</param>
        /// <param name="streamName">Имя потока</param>
        /// <param name="numbAttemps">Количество попыток переподписок на поток (-1 - не ограниченое кол-во переподписок)</param>
        public ESDBSubscriberHostedService(EventStoreClient client, IDbContextFactory<TContext> contextFactory, ILogger logger, string streamName, int numbAttemps = -1) : base(client, contextFactory, logger, streamName, numbAttemps)
        {
        }


        /// <summary>
        /// Запуск
        /// </summary>
        /// <param name="stoppingToken">Токен отмены</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            return SubscribeStreamAsync(FromStream.End);
            //return Task.CompletedTask;
        }

        /// <summary>
        /// Остановка
        /// </summary>
        /// <param name="stoppingToken">Токен отмены</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            Dispose();
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
                if (_subscriber != null)
                {
                    _subscriber.Dispose();
                }
            }
        }
    }
}