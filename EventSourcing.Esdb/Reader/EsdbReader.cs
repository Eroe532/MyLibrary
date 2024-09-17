using EventSourcing.Converter;
using EventSourcing.Esdb.Extensions;
using EventSourcing.Events;
using EventSourcing.Reader;

using EventStore.Client;

namespace EventSourcing.Esdb.Reader;

/// <summary>
/// Класс для чтения из EventStoreDB
/// </summary>
public class EsdbReader<TMetadata> : ESReader<TMetadata> where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
{

    /// <summary>
    /// Клиент EventStoreDB
    /// </summary>
    protected readonly EventStoreClient _client;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="streamName">Имя потока</param>
    /// <param name="eventConverter">Класс для преобразования в ивенты</param>
    public EsdbReader(EventStoreClient client, string streamName, IEventConverter<TMetadata> eventConverter) : base(streamName, eventConverter)
    {
        _client = client;
    }

    /// <inheritdoc/>
    protected override async Task<IEnumerable<IEventJson>> ReadEventJsonByStreamAsync(string streamName, ESDirection direction = ESDirection.Backwards, ulong? position = null , CancellationToken cancellationToken = default)
    {
        return await ReadEventJsonByStreamAsync(_client, streamName, direction, position, cancellationToken);
    }

    /// <summary>
    /// Чтение из потока
    /// </summary>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="streamName">Имя потока</param>
    /// <param name="direction">Направление чтения</param>
    /// <param name="position">Позиция с которой начинать чтение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    internal static async Task<IEnumerable<IEventJson>> ReadEventJsonByStreamAsync(EventStoreClient client, string streamName, ESDirection direction = ESDirection.Backwards, ulong? position = null, CancellationToken cancellationToken = default)
    {
        return await client.ReadStreamAsync(direction.Convert(), streamName,
                    position ?? (direction == ESDirection.Forwards ? StreamPosition.Start : StreamPosition.End),
                    resolveLinkTos: true,
                    cancellationToken: cancellationToken)
            .Select(EsdbExtension.Convert).ToListAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<ulong> GetLastEventPositionAsync(string? streamName = null, CancellationToken cancellationToken = default)
    {
        return await GetLastEventPositionAsync(_client, streamName??_streamName, cancellationToken);
    }

    /// <summary>
    /// Метод получение позиции последнего события конкретного потока
    /// </summary>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="streamName">Имя потока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Позиция</returns>
    public async static Task<ulong> GetLastEventPositionAsync(EventStoreClient client, string streamName, CancellationToken cancellationToken = default)
    {
        return (await client.ReadStreamAsync(Direction.Backwards, streamName, StreamPosition.End, maxCount: 1, resolveLinkTos: true, cancellationToken: cancellationToken).FirstAsync(cancellationToken)).Event.EventNumber;
    }

    /// <summary>
    /// Проверка на наличие потока
    /// </summary>
    /// <returns></returns>
    public override async Task<bool> CheckStreamAsync()
    {
        var result = _client.ReadStreamAsync(
            Direction.Forwards,
            _streamName,
            revision: 10,
            maxCount: 20);
        return await result.ReadState != ReadState.StreamNotFound;
    }
}
