using System;

using EventSourcing.Converter;
using EventSourcing.DependencyInjection;
using EventSourcing.Esdb.Extensions;
using EventSourcing.Esdb.Reader;
using EventSourcing.Events;
using EventSourcing.Reader;
using EventSourcing.Subscriber;

using EventStore.Client;

using Microsoft.Extensions.Logging;

using static EventStore.Client.EventStoreClient;
using static EventStore.Client.StreamMessage;

namespace EventSourcing.Esdb.Subscriber;

/// <summary>
/// Класс для слушателя(подписчика на поток) в EventStoreDB Который записывает в бд информацию
/// </summary>
public abstract class EsdbSubscriber<TMetadata> : ESSubscriber<TMetadata> where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
{

    /// <summary>
    /// Клиент EventStoreDB
    /// </summary>
    protected readonly EventStoreClient _client;

    /// <summary>
    /// подписчик
    /// </summary>
    protected StreamSubscriptionResult? _subscription = null;


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="factory">Фабрика Репозиториев Event Sourcing</param>
    /// <param name="logger">Логгер</param>
    /// <param name="streamId">Идентификатор потока</param>
    /// <param name="numbAttempts">Количество попыток переподписок на поток (-1 - не ограниченное кол-во переподписок)</param>
    public EsdbSubscriber(EventStoreClient client, IEventSourcingFactory<TMetadata> factory, ILogger logger, int streamId, int numbAttempts = -1)
        : base(factory, logger, streamId, numbAttempts)
    {
        _client = client;
    }

    /// <inheritdoc/>
    public override async Task<ulong> GetLastPosition(CancellationToken cancellationToken = default)
    {
        return (await _client.ReadStreamAsync(Direction.Backwards, _streamName, StreamPosition.End, maxCount: 1, resolveLinkTos: true)
            .FirstAsync()).Event.EventNumber;
    }

    /// <summary>
    /// метод подписки на получение Событий из EventStoreDB (по 1 потоку)
    /// </summary>
    /// <param name="position">позиция в потоке с которой начинаем смотреть (0ul - начало ulong.maxValue - конец)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    protected override async Task SubscribeToStreamAsync(ulong position, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => Subscribe(position), cancellationToken).ConfigureAwait(false);
    }

    private void Subscribe(ulong position, CancellationToken cancellationToken = default)
    {
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
        SubscribeToStream(position, cancellationToken);
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
    }

    private async Task SubscribeToStream(ulong position, CancellationToken cancellationToken = default)
    {
    Subscribe:
        try
        {
            await using var subscription = _client.SubscribeToStream(
                _streamName,
                position == ulong.MaxValue ? FromStream.End : FromStream.After(position),
                resolveLinkTos: true,
                cancellationToken: cancellationToken
            );
            _subscription = subscription;
            await foreach (var message in subscription.Messages.WithCancellation(cancellationToken))
            {
                switch (message)
                {
                    case Event(var evnt):
                        {
                            position = evnt.OriginalEventNumber;
                            SubscribeState = State.Process;
                            await Handler(evnt.Convert(), cancellationToken);
                            SubscribeState = State.Idle;
                            break;
                        }
                }
            }
        }
        catch (Exception exception)
        {
            if (_numbAttempts == 0)
            {
                SubscribeState = State.Drop;
                _logger.LogCritical(exception, $"Подписчике {this} ({_streamName}) количество достиг максимального количества перезапусков \n{exception?.Message ?? ""}");
            }
            else
            {
                if (exception is DiscoveryException)
                {
                    _logger.LogError(exception, $"Потеряно соединение с EventStoreDB");
                }
                else if ((exception is ObjectDisposedException or OperationCanceledException)
                    || (exception is Grpc.Core.RpcException rpc && rpc.StatusCode == Grpc.Core.StatusCode.Cancelled))
                {
                    _logger.LogWarning(exception, "Подписка была отменена");
                }
                else
                {
                    _logger.LogError(exception, $"В подписчике {this} ({_streamName}) Возникла ошибка в подписчике:\n{exception?.Message ?? string.Empty}");
                }
                SubscribeState = State.Inactive;
                goto Subscribe;
            }
        }
    }

    /// <inheritdoc/>
    protected override async Task<IEnumerable<IEventJson>> ReadEventByStream(ulong lastPosition, CancellationToken cancellationToken = default)
    {
        return await EsdbReader<TMetadata>.ReadEventJsonByStreamAsync(_client, _streamName, ESDirection.Forwards, lastPosition + 1, cancellationToken);
    }
}
