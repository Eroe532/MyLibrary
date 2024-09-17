namespace EventSoursing.Events;

/// <summary>
/// Интерфейс для события
/// </summary>
public interface IEvent<Tmetadata> where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
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
    /// Десериализация метаданных из Json
    /// </summary>
    /// <param name="metadata">Набор байтов</param>
    /// <param name="position">Позиция</param>
    /// <param name="changeDate">Дата изменения</param>
    void AddMetadata(byte[] metadata, ulong position, DateTime changeDate);

    /// <summary>
    /// Десериализация метаданных из Json
    /// </summary>
    /// <param name="evnt">событие</param>
    void AddMetadata(IEventJson evnt);


    /// <summary>
    /// Добавление класса методанных с содержимым
    /// </summary>
    /// <param name="metadata"></param>
    void AddMetadata(Tmetadata metadata);
}
