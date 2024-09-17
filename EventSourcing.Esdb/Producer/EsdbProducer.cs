using EventSourcing.Esdb.Extensions;
using EventSourcing.Events;
using EventSourcing.Producer;

using EventStore.Client;

namespace EventSourcing.Esdb.Producer;

/// <summary>
/// Класс для записи в EventStoreDB
/// </summary>
public class EsdbProducer<TMetadata> : ESProducer<TMetadata> where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
{
    private readonly EventStoreClient _client;
    /// <summary>
    /// Имя потока
    /// </summary>
    private readonly string _streamName;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="client">Клиент Esdb</param>
    /// <param name="streamName">Имя потока</param>
    public EsdbProducer(EventStoreClient client, string streamName) : base()
    {
        _client = client;
        _streamName = streamName;

    }

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public override async Task Save(CancellationToken cancellationToken)
    {
        var evnts = new List<EventData>();
        foreach (var evt in _changes)
        {
            evnts.Add(evt.EventData());
        }

        var _ = await Append(_client, _streamName, evnts, StreamState.StreamExists, cancellationToken);
    }

    /// <summary>
    /// запись в EventStoreDB
    /// </summary>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="StreamName">Имя потока</param>
    /// <param name="evnt">Список событий</param>
    /// <param name="streamState">Ожидаемое состояние потока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    static async Task<IWriteResult> Append(EventStoreClient client, string StreamName, IEnumerable<EventData> evnt, StreamState streamState, CancellationToken cancellationToken)
    {
        return await client.AppendToStreamAsync(
            StreamName,
            streamState,
            evnt,
            cancellationToken: cancellationToken);
    }
}
