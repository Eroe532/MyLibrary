using EventStore.Client;

using EventStoreDBLibrary.Events;

namespace EventStoreDBLibrary.ESDB
{
    /// <summary>
    /// Класс для чтения из EventStoreDB
    /// </summary>
    public abstract class ESDBReader<TEventConverter, Tmetadata> where TEventConverter : EventConverter<Tmetadata> where Tmetadata : EventMetadata, new()
    {
        /// <summary>
        /// Имя потока
        /// </summary>
        protected readonly string _streamName;

        /// <summary>
        /// Клиент EventStoreDB
        /// </summary>
        protected EventStoreClient _client;

        /// <summary>
        /// Класс для преобразования в ивенты
        /// </summary>
        protected TEventConverter _eventConverter;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="streamName">Имя потока</param>
        /// <param name="eventConverter">Класс для преобразования в ивенты</param>
        public ESDBReader(EventStoreClient client, string streamName, TEventConverter eventConverter)
        {
            _streamName = streamName;
            _eventConverter = eventConverter;
            _client = client;
        }

        /// <summary>
        /// Чтение из EventStoreDB
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="streamName">Имя потока</param>
        /// <param name="direction">Направление чтения</param>
        /// <param name="position">Позиция с которой начинать чтение</param>
        /// <param name="withCancelEvents">С применением аннулирования или нет</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async static Task<IAsyncEnumerable<ResolvedEvent>> ReadStream(EventStoreClient client, string streamName, Direction direction = Direction.Backwards, StreamPosition? position = null,
            bool withCancelEvents = false, CancellationToken cancellationToken = default)
        {
            if (withCancelEvents)
            {
                var typeCancelEvent = new Dictionary<Guid, string>();
                var readStreamResult = await Task.Run(() => client.ReadStreamAsync(Direction.Backwards, streamName, position ?? StreamPosition.End, resolveLinkTos: true, cancellationToken: cancellationToken));
                var cancelEvents = new HashSet<Guid>();
                var result = new List<ResolvedEvent>();
                await foreach (var resolvedEvent in readStreamResult)
                {
                    if (cancelEvents.Contains(resolvedEvent.Event.EventId.ToGuid()))
                    {
                        typeCancelEvent.Add(resolvedEvent.Event.EventId.ToGuid(), resolvedEvent.Event.EventType);
                        continue;
                    }
                    switch (resolvedEvent.Event.EventType)
                    {
                        case "CancelEventsV1":
                            {
                                var @event = (CancelEventsV1<Tmetadata>)resolvedEvent.ConverterResolvedEventToEvent<Tmetadata>(typeof(CancelEventsV1<Tmetadata>));
                                foreach (var id in @event.CanceledEventsIds)
                                {
                                    cancelEvents.Add(id);
                                }
                                break;
                            }
                        default:
                            {
                                result.Add(resolvedEvent);
                                break;
                            }
                    }
                }
                return direction == Direction.Forwards ? result.ToAsyncEnumerable().Reverse() : result.ToAsyncEnumerable();
            }
            else
            {
                return await Task.Run(() => client.ReadStreamAsync(direction, streamName, position ?? (direction == Direction.Forwards ? StreamPosition.Start : StreamPosition.End), resolveLinkTos: true, cancellationToken: cancellationToken));
            }
        }

        /// <summary>
        /// Чтение из EventStoreDB
        /// </summary>
        /// <param name="direction">Направление чтения</param>
        /// <param name="position">Позиция с которой начинать чтение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="withCancelEvents">С применением аннулирования или нет</param>
        /// <returns></returns>
        public async Task<IEnumerable<IEvent<Tmetadata>>> ReadEventByStream(Direction direction = Direction.Backwards, ulong position = 0u, bool withCancelEvents = false, CancellationToken cancellationToken = default)
        {
            var evnts = await ReadStream(_client, _streamName, direction, position, withCancelEvents, cancellationToken);
            return await _eventConverter.ConverterStreamResultToListEvent(evnts);
        }

        /// <summary>
        /// Чтение из EventStoreDB
        /// </summary>
        /// <param name="direction">Направление чтения</param>
        /// <param name="position">Позиция с которой начинать чтение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="withCancelEvents">С применением аннулирования или нет</param>
        /// <returns></returns>
        public async Task<IEnumerable<(IEvent<Tmetadata> @event, ulong position)>> ReadEventWithPositionByStream(Direction direction = Direction.Backwards, ulong position = 0u, bool withCancelEvents = false, CancellationToken cancellationToken = default)
        {
            var evnts = await ReadStream(_client, _streamName, direction, position, withCancelEvents, cancellationToken);
            return await _eventConverter.ConverterStreamResultToListEventWithPosition(evnts);
        }
        /// <summary>
        /// Чтение из EventStoreDB
        /// </summary>
        /// <param name="direction">Направление чтения</param>
        /// <param name="position">Позиция с которой начинать чтение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="withCancelEvents">С применением аннулирования или нет</param>
        /// <param name="streamNameByPosition">Позиция из какого потока если нул то из того из которого читается</param>
        /// <returns></returns>
        public async Task<(IEnumerable<IEvent<Tmetadata>> events, ulong position)> ReadEventWithLastPositionByStream(Direction direction = Direction.Backwards, StreamPosition? position = null,
            string? streamNameByPosition = null, bool withCancelEvents = false, CancellationToken cancellationToken = default)
        {
            var evnts = await ReadStream(_client, _streamName, direction, position, withCancelEvents, cancellationToken);
            return await _eventConverter.ConverterStreamResultToListEventWithLastPosition(evnts,
                streamNameByPosition);
        }
        /// <summary>
        /// Чтение из EventStoreDB
        /// </summary>
        /// <param name="direction">Направление чтения</param>
        /// <param name="position">Позиция с которой начинать чтение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="streamNameByPosition">Позиция из какого потока если нул то из того из которого читается</param>
        /// <returns></returns>
        public async Task<(IEnumerable<IEvent<Tmetadata>> events, ulong position, Dictionary<Guid, string> cancelEventsNames)> ReadEventWithCancelEventNameByStream(Direction direction = Direction.Backwards, StreamPosition? position = null,
            string? streamNameByPosition = null, CancellationToken cancellationToken = default)
        {
            var typeCancelEvent = new Dictionary<Guid, string>();
            var readStreamResult = await Task.Run(() => _client.ReadStreamAsync(Direction.Backwards, _streamName, position ?? StreamPosition.End, resolveLinkTos: true, cancellationToken: cancellationToken));
            var cancelEvents = new HashSet<Guid>();
            var result = new List<ResolvedEvent>();
            await foreach (var resolvedEvent in readStreamResult)
            {
                if (cancelEvents.Contains(resolvedEvent.Event.EventId.ToGuid()))
                {
                    typeCancelEvent.Add(resolvedEvent.Event.EventId.ToGuid(), resolvedEvent.Event.EventType);
                    continue;
                }
                switch (resolvedEvent.Event.EventType)
                {
                    case "CancelEventsV1":
                        {
                            var @event = (CancelEventsV1<Tmetadata>)_eventConverter.ConverterResolvedEventToEvent(resolvedEvent);
                            foreach (var id in @event.CanceledEventsIds)
                            {
                                cancelEvents.Add(id);
                            }
                            break;
                        }
                    default:
                        {
                            result.Add(resolvedEvent);
                            break;
                        }
                }
            }
            var evnts = direction == Direction.Forwards ? result.ToAsyncEnumerable().Reverse() : result.ToAsyncEnumerable();
            var res = await _eventConverter.ConverterStreamResultToListEventWithLastPosition(evnts,
                streamNameByPosition);
            return (res.events, res.position, typeCancelEvent);
        }


        /// <summary>
        /// Чтение из EventStoreDB
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<ulong> GetLastEventPositionAsync(CancellationToken cancellationToken = default) =>
           await GetLastEventPositionAsync(_client, _streamName, cancellationToken);

        /// <summary>
        /// Метод получение позиции последнего Событиеа конкретного потока
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="streamName">Имя потока</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Позиция</returns>
        public async static Task<ulong> GetLastEventPositionAsync(EventStoreClient client, string streamName, CancellationToken cancellationToken = default)
        {
            return (await client.ReadStreamAsync(Direction.Backwards, streamName, StreamPosition.End, maxCount: 1, resolveLinkTos: true, cancellationToken: cancellationToken).FirstAsync(cancellationToken)).Event.EventNumber;
        }

        /// <summary>
        /// Проверка на наличее потока
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckStreamAsync()
        {
            return await CheckStreamAsync(_client, _streamName);
        }

        /// <summary>
        /// Проверка на наличее потока
        /// </summary>
        /// <param name="client">Клиент EventStoreDB</param>
        /// <param name="streamName">Имя потока</param>
        /// <returns></returns>
        private async static Task<bool> CheckStreamAsync(EventStoreClient client, string streamName)
        {
            var result = client.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                revision: 10,
                maxCount: 20);
            return await result.ReadState != ReadState.StreamNotFound;
        }
    }
}