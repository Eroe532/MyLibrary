namespace EventSoursing.Subscriber;

/// <summary>
/// Класс для слушателя(подписчика на поток) в EventStoreDB Который записывает в бд информацию
/// </summary>
public interface IESSubscriber
{
    /// <summary>
    /// Идентификатор потока
    /// </summary>
    public int StreamId { get; }

    /// <summary>
    /// Имя потока
    /// </summary>
    public string StreamName { get; }

    /// <summary>
    /// метод ожидания синхронизации потока стора и целевой рид модели (true - все ок / false - подписчик лежит)
    /// </summary>
    /// <returns></returns>
    public Task SyncWaiting(int numbAttemps = 10, bool waitEnd = false);

    /// <summary>
    /// обертка на проверку акульного состояния актульной модели данных на актульность
    /// (проверяет актуальность данных в posgre, при необходимости дочитывает события из стора)
    /// </summary>
    /// <returns></returns>
    public Task<bool> IsActualStateAsync();

    /// <summary>
    /// Функция запускаемая при старте подписки
    /// </summary>
    public Task SubscribeStartAction();

    /// <summary>
    /// метод подписки на получение Событиеов из EventStoreDB (по 1 потоку)
    /// </summary>
    public Task SubscribeStreamAsync();
}
