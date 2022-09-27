using System.Text.Json.Serialization;
using System.Text.Json;
using EventStore.Client;

namespace EventStoreDBLibrary.Events
{
    /// <summary>
    /// Абстрактный класс с методами общими для всех событий
    /// </summary>
    public abstract class Event<Tmetadata> : IEvent<Tmetadata> where Tmetadata : EventMetadata, new()
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

        /// <inheritdoc/>
        public void AddMetadata(byte[] metadata, DateTime changeDate)
        {
            Metadata = JsonSerializer.Deserialize<Tmetadata>(metadata)!;
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

        /// <inheritdoc/>
        public EventData EventData()
        {
            return new EventData(
                Uuid.FromGuid(EventId),
                GetType().Name,
                JsonSerializeData(),
                JsonSerializeMetadata()
            );
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <param name="metadata">Методанные</param>
        protected Event(Guid eventId, Tmetadata metadata)
        {
            EventId = eventId;
            Metadata = metadata;
            Metadata.EventId = eventId;
            Metadata.EventTypeName = (dynamic)this.GetType().Name;
            if (Metadata.ParentEventId == Guid.Empty)
            {
                Metadata.ParentEventId = eventId;
            }
        }

    }
}