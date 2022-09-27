using EventStore.Client;

using EventStoreDBLibrary.Events;

namespace EventStoreDBLibrary.ESDB
{
    /// <summary>
    /// Класс для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)
    /// в List IEvent  (список классов Событиеов)
    /// </summary>
    public abstract partial class EventConverter<Tmetadata> where Tmetadata : EventMetadata
    {
        /// <summary>
        /// Метод для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)
        /// в List IEvent (список классов Событиеов)
        /// </summary>
        /// <param name="evnts">ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)</param>
        /// <returns>List IEvent  (список классов Событиеов)</returns>
        public async Task<IEnumerable<IEvent<Tmetadata>>> ConverterStreamResultToListEvent(IAsyncEnumerable<ResolvedEvent> evnts)
        {
            return await Task.Run(async () =>
            {
                var events = new List<IEvent<Tmetadata>>();
                foreach (var e in await evnts.ToListAsync())
                {
                    var ec = ConverterResolvedEventToEvent(e);

                    if (ec != null)
                    {
                        events.Add(ec);
                    }
                }
                return events;
            });
        }

        /// <summary>
        /// Метод для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)
        /// в List IEvent  (список классов Событиеов)
        /// </summary>
        /// <param name="evnts">ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)</param>
        /// <returns>List IEvent  (список классов Событиеов)</returns>
        public async Task<IEnumerable<(IEvent<Tmetadata> @event, ulong position)>> ConverterStreamResultToListEventWithPosition(IAsyncEnumerable<ResolvedEvent> evnts)
        {
            return await Task.Run(async () =>
            {
                var events = new List<(IEvent<Tmetadata> @event, ulong position)>();
                foreach (var e in await evnts.ToListAsync())
                {
                    var ec = ConverterResolvedEventToEvent(e);
                    if (ec != null)
                    {
                        events.Add((ec, e.Event.EventNumber.ToUInt64()));
                    }
                }
                return events;
            });
        }

        /// <summary>
        /// Метод для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)
        /// в List IEvent  (список классов Событиеов)
        /// </summary>
        /// <param name="evnts">ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)</param>
        /// <param name="streamNameByPosition">Названия потоко в  котором необходимо отслеживать позицию</param>
        /// <returns>List IEvent  (список классов Событиеов)</returns>
        public async Task<(List<IEvent<Tmetadata>> events, ulong position)> ConverterStreamResultToListEventWithLastPosition(IAsyncEnumerable<ResolvedEvent> evnts,
            string? streamNameByPosition)
        {
            return await Task.Run(async () =>
            {
                var events = (events: new List<IEvent<Tmetadata>>(), position: 0ul);
                foreach (var e in await evnts.ToListAsync())
                {
                    var ec = ConverterResolvedEventToEvent(e);
                    if (ec != null)
                    {
                        events.events.Add(ec);
                        var position = e.Event.EventNumber.ToUInt64();
                        if (streamNameByPosition == null)
                        {
                            events.position = e.OriginalEventNumber.ToUInt64();
                        }
                        else if (e.Event.EventStreamId == streamNameByPosition)
                        {
                            events.position = e.Event.EventNumber.ToUInt64();
                        }
                    }

                }
                return events;
            });
        }


        /// <summary>
        /// для преоброзования ResolvedEvent (класс в виде которого предсатвлен один Событие в ESDB)
        /// в IEvent (класс Событиеа)
        /// </summary>
        /// <param name="evnt">ResolvedEvent (класс в виде которого предсатвлен один Событие в ESDB)</param>
        /// <returns>IEvent (класс Событиеа)</returns>
        public abstract IEvent<Tmetadata> ConverterResolvedEventToEvent(ResolvedEvent evnt);
    }
}