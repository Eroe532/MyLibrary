namespace EventSoursing.Events;

/// <summary>
/// Итерфейс для Методанных
/// </summary>
public interface ICloneMetadata<Tthis> where Tthis : IEventMetadata
{
    /// <summary>
    /// Клоннирование
    /// </summary>
    /// <returns></returns>
    public Tthis Clone();
}
