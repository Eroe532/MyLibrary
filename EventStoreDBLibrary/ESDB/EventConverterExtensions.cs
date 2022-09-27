using System.Text.Json;

using EventStore.Client;

using EventStoreDBLibrary.Events;
using EventStoreDBLibrary.Exeptions;

namespace EventStoreDBLibrary.ESDB
{
    /// <summary>
    /// Класс для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из ESDB)
    /// в List IEvent  (список классов Событиеов)
    /// </summary>
    public static class EventConverterExtensions
    {
        /// <summary>
        /// Метод для преоброзования ResolvedEvent (класс в виде которого предсатвлен один Событие в ESDB)
        /// в T (класс Событиеа)
        /// </summary>
        /// <param name="evnt">ResolvedEvent (класс в виде которого предсатвлен один Событие в ESDB)</param>
        /// <returns>T (класс Событиеа)</returns>
        public static T ConverterResolvedEventToEvent<T, Tmetadata>(this ResolvedEvent evnt) where T : IEvent<Tmetadata> where Tmetadata : EventMetadata
        {
            var e = JsonSerializer.Deserialize<T>(evnt.Event.Data.ToArray());
            if (e == null)
            {
                throw new EventParseException(evnt.Event.EventType);
            }
            e!.EventId = evnt.Event.EventId.ToGuid();
            e.AddMetadata(evnt.Event.Metadata.ToArray(), evnt.Event.Created);
            return e;
        }

        /// <summary>
        /// Метод для преоброзования ResolvedEvent (класс в виде которого предсатвлен один Событие в ESDB)
        /// в T (класс Событиеа)
        /// </summary>
        /// <param name="evnt">ResolvedEvent (класс в виде которого предсатвлен один Событие в ESDB)</param>
        /// <param name="type">Тип Событиеа</param>
        /// <returns>Класс Событиеа</returns>
        public static IEvent<Tmetadata> ConverterResolvedEventToEvent<Tmetadata>(this ResolvedEvent evnt, Type type) where Tmetadata : EventMetadata
        {
            var e = (IEvent<Tmetadata>?)JsonSerializer.Deserialize(evnt.Event.Data.ToArray(), type);
            if (e == null)
            {
                throw new EventParseException(evnt.Event.EventType);
            }
            e!.EventId = evnt.Event.EventId.ToGuid();
            e.AddMetadata(evnt.Event.Metadata.ToArray(), evnt.Event.Created);
            return e;
        }


        /// <summary>
        /// Сортировка по Effective date
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<IEvent<Tmetadata>> OrderByEffectiveDate<Tmetadata>(this IEnumerable<IEvent<Tmetadata>> list) where Tmetadata : EventMetadata
        {
            return list.OrderBy(e => e.Metadata.EffectiveDate);
        }

        /// <summary>
        /// Сортировка по Effective date
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<IEvent<Tmetadata>> OrderByEffectiveDate<Tmetadata>(this IEnumerable<(IEvent<Tmetadata> events, ulong position)> list) where Tmetadata : EventMetadata
        {
            return list.Select(m => m.events).OrderBy(e => e.Metadata.EffectiveDate);
        }
    }
}