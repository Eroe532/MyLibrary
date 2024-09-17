using EventSourcing.Events;

using EventStore.Client;

namespace EventSourcing.Esdb.Events;

/// <summary>
/// Событие в Json
/// </summary>
public class EventJson : IEventJson
{
    ///<inheritdoc/>
    public Guid EventId { get; init; }

    ///<inheritdoc/>
    public byte[] Data { get; init; }

    ///<inheritdoc/>
    public byte[] Metadata { get; init; }

    ///<inheritdoc/>
    public ulong Position { get; init; }

    ///<inheritdoc/>
    public DateTime CreatedTime { get; init; }

    ///<inheritdoc/>
    public string EventType { get; init; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="resolvedEvent"></param>
    private EventJson(ResolvedEvent resolvedEvent)
    {
        EventId = resolvedEvent.Event.EventId.ToGuid();
        Data = resolvedEvent.Event.Data.ToArray();
        Metadata = resolvedEvent.Event.Metadata.ToArray();
        Position = resolvedEvent.Event.EventNumber;
        CreatedTime = resolvedEvent.Event.Created;
        EventType = resolvedEvent.Event.EventType;
    }

    /// <summary>
    /// Преобразование
    /// </summary>
    /// <param name="resolvedEvent"></param>
    public static implicit operator EventJson(ResolvedEvent resolvedEvent) { return new EventJson(resolvedEvent); }
}
