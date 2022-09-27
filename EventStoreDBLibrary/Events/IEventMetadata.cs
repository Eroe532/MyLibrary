namespace EventStoreDBLibrary.Events
{
    /// <summary>
    /// Итерфейс для Методанных
    /// </summary>
    interface IEventMetadata
    {
        /// <summary>
        /// Id события
        /// </summary>
        Guid EventId { get; set; }


        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Название Событиеа
        /// </summary>
        public string EventTypeName { get; set; }

        /// <summary>
        /// Идентификатор Событиеа Родителя
        /// </summary>
        public Guid ParentEventId { get; set; }

        /// <summary>
        /// Id Аггрегата
        /// </summary>
        int AggregateId { get; set; }

        /// <summary>
        /// Дата Сохранения
        /// </summary>
        DateTime ChangeDate { get; set; }

        /// <summary>
        /// Идентификатор того кто изменил
        /// </summary>
        int WorkerChangeBy { get; set; }
    }
}