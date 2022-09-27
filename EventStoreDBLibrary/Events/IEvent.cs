using EventStore.Client;

namespace EventStoreDBLibrary.Events
{
    /// <summary>
    /// Интерфейс для события
    /// </summary>
    public interface IEvent<Tmetadata> where Tmetadata : EventMetadata
    {
        /// <summary>
        /// ID события
        /// </summary>
        Guid EventId { get; set; }

        /// <summary>
        /// Метаданные события
        /// </summary>
        Tmetadata Metadata { get; set; }

        /// <summary>
        /// Сериализация Данных (полей этого класса) в Json
        /// </summary>
        /// <returns></returns>
        byte[] JsonSerializeData();

        /// <summary>
        /// Сериализация Данных (полей этого класса) в Json
        /// </summary>
        /// <returns></returns>
        string JsonSerializeDataToString();

        /// <summary>
        /// Сериализация методанных в Json
        /// </summary>
        /// <returns></returns>
        byte[] JsonSerializeMetadata();

        /// <summary>
        /// Преоброзование класса к классу T
        /// </summary>
        /// <returns></returns>
        EventData EventData();

        /// <summary>
        /// Десериализация метаданных из Json
        /// </summary>
        /// <param name="metadata">Набор байтов</param>
        /// <param name="changeDate">Дата изменения</param>
        void AddMetadata(byte[] metadata, DateTime changeDate);

        /// <summary>
        /// Добавление класса методанных с содержимым
        /// </summary>
        /// <param name="metadata"></param>
        void AddMetadata(Tmetadata metadata);
    }
}