using EventSourcing.Converter;
using EventSourcing.DependencyInjection;
using EventSourcing.Events;

using EventStore.Client;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventSourcing.Esdb.Subscriber;

/// <summary>
/// Класс для слушателя(подписчика на поток) в EventStoreDB Который записывает в бд информацию
/// </summary>
public abstract class EsdbSubscriberHostedService<TMetadata> : EsdbSubscriber<TMetadata>, IHostedService, IDisposable where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
{

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="factory">Фабрика Репозиториев Event Sourcing</param>
    /// <param name="logger">Логгер</param>
    /// <param name="streamId">Идентификатор потока</param>
    /// <param name="numbAttempts">Количество попыток переподписок на поток (-1 - не ограниченное кол-во переподписок)</param>
    public EsdbSubscriberHostedService(EventStoreClient client, IEventSourcingFactory<TMetadata> factory, ILogger logger, int streamId, int numbAttempts = -1)
        : base(client, factory, logger, streamId, numbAttempts)
    {
    }


    /// <summary>
    /// Запуск
    /// </summary>
    /// <param name="stoppingToken">Токен отмены</param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken stoppingToken)
    {
        return SubscribeStreamAsync(stoppingToken);
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
            _subscription?.Dispose();
        }
    }
}
