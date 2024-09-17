using System.Text.Json.Serialization;

namespace EventSoursing.Events;

/// <summary>
/// Класс с методанными
/// </summary>
public class EventMetadata : IEventMetadata, ICloneMetadata<EventMetadata>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="aggregateId">Id Аггрегата</param>
    /// <param name="effectiveDate">Дата вступления в силу</param>
    /// <param name="parentEventId">Идентификатор Событиеа Родителя</param>
    /// <param name="workerChangeBy">Идентификатор работника</param>
    public EventMetadata(int aggregateId, DateTime effectiveDate, Guid parentEventId, int workerChangeBy)
    {
        AggregateId = aggregateId;
        EffectiveDate = effectiveDate;
        ParentEventId = parentEventId;
        WorkerChangeBy = workerChangeBy;
        EventTypeName = "";
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="eventId">идентификатор</param>
    /// <param name="aggregateId">Id Аггрегата</param>
    /// <param name="changeDate">Втупление в силу</param>
    /// <param name="effectiveDate">Дата вступления в силу</param>
    /// <param name="eventTypeName">Название типа</param>
    /// <param name="parentEventId">Идентификатор Событиеа Родителя</param>
    /// <param name="workerChangeBy">Идентификатор работника</param>
    public EventMetadata(Guid eventId, int aggregateId, DateTime changeDate, DateTime effectiveDate, string eventTypeName, Guid parentEventId, int workerChangeBy)
    {
        EventId = eventId;
        AggregateId = aggregateId;
        ChangeDate = changeDate;
        EffectiveDate = effectiveDate;
        EventTypeName = eventTypeName;
        ParentEventId = parentEventId;
        WorkerChangeBy = workerChangeBy;
    }


    /// <summary>
    /// Конструктор
    /// </summary>
    public EventMetadata()
    {
        EventTypeName = "";
    }

    /// <inheritdoc/>
    public Guid EventId { get; set; }

    /// <inheritdoc />
    public int AggregateId { get; set; }

    /// <inheritdoc/>
    [JsonIgnore]
    public ulong Position { get; set; }

    /// <inheritdoc/>
    [JsonIgnore]
    public DateTime ChangeDate { get; set; }

    /// <inheritdoc/>
    public DateTime EffectiveDate { get; set; }

    /// <inheritdoc/>
    public string EventTypeName { get; set; }

    /// <inheritdoc/>
    public Guid ParentEventId { get; set; }

    /// <inheritdoc/>
    public int WorkerChangeBy { get; set; }

    /// <summary>
    /// Клоннирование
    /// </summary>
    /// <returns></returns>
    public EventMetadata Clone()
    {
        return new EventMetadata(EventId, AggregateId, ChangeDate, EffectiveDate, EventTypeName, ParentEventId, WorkerChangeBy);
    }
}
