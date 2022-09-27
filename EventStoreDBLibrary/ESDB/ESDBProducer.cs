using EventStore.Client;

using EventStoreDBLibrary.Events;

namespace EventStoreDBLibrary.ESDB
{
    /// <summary>
    /// Класс для даписи в EventStoreDB
    /// </summary>
    public abstract class ESDBProducer<Tmetadata> where Tmetadata : EventMetadata
    {
        private readonly EventStoreClient _client;
        /// <summary>
        /// Имя потока
        /// </summary>
        private readonly string _streamName;

        /// <summary>
        /// Список изменений
        /// </summary>
        private readonly List<IEvent<Tmetadata>> _changes = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Клиент ESDB</param>
        /// <param name="streamName">Имя потока</param>
        public ESDBProducer(EventStoreClient client, string streamName)
        {
            _client = client;
            _streamName = streamName;

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
        public async Task<IWriteResult> Save(CancellationToken cancellationToken)
        {
            var evnts = new List<EventData>();
            foreach (var evt in _changes)
            {
                evnts.Add(evt.EventData());
            }
            var writeResult = await Append(_client, _streamName, evnts, StreamState.StreamExists, cancellationToken);
            return writeResult;
        }

        /// <summary>
        /// запись в EventStoreDB
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="StreamName">Имя потока</param>
        /// <param name="evnt">Список событий</param>
        /// <param name="streamState">Ожидаемое состояние потока</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        static async Task<IWriteResult> Append(EventStoreClient client, string StreamName, IEnumerable<EventData> evnt, StreamState streamState, CancellationToken cancellationToken)
        {
            return await client.AppendToStreamAsync(
                StreamName,
                streamState,
                evnt,
                cancellationToken: cancellationToken);
        }
    }
}