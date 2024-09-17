using EventSoursing.Events;

namespace EventSoursing.Producer;

/// <summary>
/// Класс для даписи в EventStoreDB
/// </summary>
public interface IESProducer<Tmetadata> where Tmetadata : IEventMetadata, new()
{
    /// <summary>
    /// возвращает true если есть не сохраненные изменения
    /// false во всех остальных случаях
    /// </summary>
    /// <returns></returns>
    public bool IsChange();

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public Task Save(CancellationToken cancellationToken);
}
