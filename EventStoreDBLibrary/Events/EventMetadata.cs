using System.Text.Json.Serialization;

namespace EventStoreDBLibrary.Events
{
    /// <summary>
    /// Класс с методанными
    /// </summary>
    public class EventMetadata : IEventMetadata
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
        public DateTime ChangeDate { get; set; }

        /// <inheritdoc/>
        public DateTime EffectiveDate { get; set; }

        /// <inheritdoc/>
        public string EventTypeName { get; set; }

        /// <inheritdoc/>
        public Guid ParentEventId { get; set; }

        /// <inheritdoc/>
        public int WorkerChangeBy { get; set; }

    }
}