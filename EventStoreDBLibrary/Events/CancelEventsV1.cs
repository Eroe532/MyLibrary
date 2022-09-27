namespace EventStoreDBLibrary.Events
{
    /// <summary>
    /// Событие аннулирования
    /// </summary>
    public class CancelEventsV1<Tmetadata> : Event<Tmetadata> where Tmetadata : EventMetadata, new()
    {
        /// <summary>
        /// Идентификаторы аннулируемых событий
        /// </summary>
        public Guid[] CanceledEventsIds { get; set; }

        /// <summary>
        /// Идентификатор Событиеа Родителя
        /// </summary>
        public Guid CanceledEventParentId { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CancelEventsV1()
        {
            CanceledEventsIds = Array.Empty<Guid>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="eventId">Идентификатор</param>
        /// <param name="metadata">Методанные</param>
        /// <param name="canceledEventsIds">Идентификаторы аннулируемых событий</param>
        /// <param name="canceledEventParentId">Идентификатор Событиеа Родителя</param>
        public CancelEventsV1(Guid eventId, Tmetadata metadata,
            Guid[] canceledEventsIds, Guid canceledEventParentId)
            : base(eventId, metadata)
        {
            CanceledEventsIds = canceledEventsIds;
            CanceledEventParentId = canceledEventParentId;
        }
    }
}