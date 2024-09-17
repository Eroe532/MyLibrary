using EventSoursing.Events;

namespace EventSoursing.Producer;

/// <summary>
/// Класс для даписи в EventStoreDB
/// </summary>
public abstract class ESProducer<Tmetadata> : IESProducer<Tmetadata> where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
{

    /// <summary>
    /// Список изменений
    /// </summary>
    private readonly List<IEvent<Tmetadata>> _changes = new();

    /// <summary>
    /// Конструктор
    /// </summary>
    public ESProducer()
    {
    }

    /// <summary>
    /// возвращает true если есть не сохраненные изменения
    /// false во всех остальных случаях
    /// </summary>
    /// <returns></returns>
    public bool IsChange()
    {
        return _changes.Count > 0;
    }

    /// <summary>
    /// Добавление Изменений
    /// </summary>
    /// <param name="evt"></param>
    protected virtual void Apply(IEvent<Tmetadata> evt)
    {
        _changes.Add(evt);
    }

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public abstract Task Save(CancellationToken cancellationToken);
}
