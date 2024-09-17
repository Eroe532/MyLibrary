using System.Text.Json;
using EventSoursing.Events;
using EventSoursing.Exeptions;

namespace EventSoursing;

/// <summary>
/// Класс для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из Esdb)
/// в List IEvent  (список классов Событиеов)
/// </summary>
public static class EventConverterExtensions
{
    /// <summary>
    /// Метод для преоброзования ResolvedEvent (класс в виде которого предсатвлен один Событие в Esdb)
    /// в T (класс Событиеа)
    /// </summary>
    /// <param name="evnt">ResolvedEvent (класс в виде которого предсатвлен один Событие в Esdb)</param>
    /// <returns>T (класс Событиеа)</returns>
    public static T ConverterResolvedEventToEvent<T, Tmetadata>(this IEventJson evnt) where T : IEvent<Tmetadata> where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
    {
        var e = JsonSerializer.Deserialize<T>(evnt.Data.ToArray()) 
            ?? throw new EventParseException(evnt.EventType);
        e!.EventId = evnt.EventId;
        e.AddMetadata(evnt);
        return e;
    }

    /// <summary>
    /// Метод для преоброзования ResolvedEvent (класс в виде которого предсатвлен один Событие в Esdb)
    /// в T (класс Событиеа)
    /// </summary>
    /// <param name="evnt">ResolvedEvent (класс в виде которого предсатвлен один Событие в Esdb)</param>
    /// <param name="type">Тип Событиеа</param>
    /// <returns>Класс Событиеа</returns>
    public static IEvent<Tmetadata> ConverterResolvedEventToEvent<Tmetadata>(this IEventJson evnt, Type type) where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
    {
        var e = (IEvent<Tmetadata>?)JsonSerializer.Deserialize(evnt.Data.ToArray(), type) 
            ?? throw new EventParseException(evnt.EventType);
        e!.EventId = evnt.EventId;
        e.AddMetadata(evnt);
        return e;
    }


    /// <summary>
    /// Сортировка по Effective date
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IEnumerable<IEvent<Tmetadata>> OrderByEffectiveDate<Tmetadata>(this IEnumerable<IEvent<Tmetadata>> list) where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
    {
        return list.OrderBy(e => e.Metadata.EffectiveDate);
    }

    /// <summary>
    /// Сортировка по Effective date
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IEnumerable<IEvent<Tmetadata>> OrderByEffectiveDate<Tmetadata>(this IEnumerable<(IEvent<Tmetadata> events, ulong position)> list) where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
    {
        return list.Select(m => m.events).OrderBy(e => e.Metadata.EffectiveDate);
    }
}
