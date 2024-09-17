using EventSoursing.Events;
using EventSoursing.Reader;

namespace EventSoursing.Reader;

/// <summary>
/// Класс для чтения из EventStoreDB
/// </summary>
public interface IESReader<Tmetadata> where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
{
    /// <summary>
    /// Чтение из EventStoreDB
    /// </summary>
    /// <param name="direction">Направление чтения</param>
    /// <param name="position">Позиция с которой начинать чтение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <param name="withCancelEvents">С применением аннулирования или нет</param>
    /// <returns></returns>
    public Task<IEnumerable<IEvent<Tmetadata>>> ReadEventByStream(ESDirection direction = ESDirection.Backwards, ulong position = 0u, bool withCancelEvents = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Чтение из EventStoreDB
    /// </summary>
    /// <param name="direction">Направление чтения</param>
    /// <param name="position">Позиция с которой начинать чтение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public Task<(IEnumerable<IEvent<Tmetadata>> events, ulong position, Dictionary<Guid, string> cancelEventsNames)> ReadEventWithCancelEventNameByStream(ESDirection direction = ESDirection.Backwards, 
        ulong? position = null,CancellationToken cancellationToken = default);


    /// <summary>
    /// Чтение из EventStoreDB
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public Task<ulong> GetLastEventPositionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверка на наличее потока
    /// </summary>
    /// <returns></returns>
    public Task<bool> CheckStreamAsync();
}
