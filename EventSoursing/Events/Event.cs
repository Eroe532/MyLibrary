using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventSoursing.Events;
/// <summary>
/// Абстрактный класс с методами общими для всех событий
/// </summary>
public abstract class Event<Tmetadata> : IEvent<Tmetadata> where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
{
    /// <inheritdoc/>
    [JsonIgnore]
    public Guid EventId { get; set; }

    /// <inheritdoc/>
    [JsonIgnore]
    public Tmetadata Metadata { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public Event()
    {
        Metadata = new Tmetadata();
    }

    /// <summary>
    /// Десериализация метаданных из Json
    /// </summary>
    /// <param name="evnt">Событие в json</param>
    public void AddMetadata(IEventJson evnt)
    {
        AddMetadata(evnt.Metadata.ToArray(), evnt.Position, evnt.CreatedTime);
    }

    /// <inheritdoc/>
    public void AddMetadata(byte[] metadata, ulong position, DateTime changeDate)
    {
        Metadata = JsonSerializer.Deserialize<Tmetadata>(metadata)!;
        Metadata.Position = position;
        Metadata.ChangeDate = changeDate;
    }

    /// <inheritdoc/>
    public void AddMetadata(Tmetadata metadata)
    {
        Metadata = metadata;
        if (Metadata.ParentEventId == Guid.Empty)
        {
            Metadata.ParentEventId = metadata.EventId;
        }
    }

    /// <inheritdoc/>
    public byte[] JsonSerializeData()
    {
        return JsonSerializer.SerializeToUtf8Bytes((dynamic)this);
    }

    /// <inheritdoc/>
    public string JsonSerializeDataToString()
    {
        return JsonSerializer.Serialize((dynamic)this);
    }

    /// <inheritdoc/>
    public byte[] JsonSerializeMetadata()
    {
        return JsonSerializer.SerializeToUtf8Bytes((dynamic)Metadata);
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="metadata">Методанные</param>
    protected Event(Guid eventId, Tmetadata metadata)
    {
        EventId = eventId;
        Metadata = metadata.Clone();
        Metadata.EventId = eventId;
        Metadata.EventTypeName = (dynamic)this.GetType().Name;
        if (Metadata.ParentEventId == Guid.Empty)
        {
            Metadata.ParentEventId = eventId;
        }
    }
}