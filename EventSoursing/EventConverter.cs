using EventSoursing.Events;

namespace EventSoursing;

/// <summary>
/// Класс для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из Esdb)
/// в List IEvent  (список классов Событиеов)
/// </summary>
public abstract class EventConverter<Tmetadata> where Tmetadata : IEventMetadata, ICloneMetadata<Tmetadata>, new()
{
    /// <summary>
    /// Метод для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из Esdb)
    /// в List IEvent (список классов Событиеов)
    /// </summary>
    /// <param name="evnts">ReadStreamResult (класс в виде которого предсатвлен результат из Esdb)</param>
    /// <returns>List IEvent  (список классов Событиеов)</returns>
    public async Task<IEnumerable<IEvent<Tmetadata>>> ConverterStreamResultToListEvent(IAsyncEnumerable<IEventJson> evnts)
    {
        var events = new List<IEvent<Tmetadata>>();
        await foreach (var e in evnts)
        {
            var ec = ConverterResolvedEventToEvent(e);

            if (ec != null)
            {
                events.Add(ec);
            }
        }

        return events;
    }
    /// <summary>
    /// Метод для преоброзования ReadStreamResult (класс в виде которого предсатвлен результат из Esdb)
    /// в List IEvent (список классов Событиеов)
    /// </summary>
    /// <param name="evnts">ReadStreamResult (класс в виде которого предсатвлен результат из Esdb)</param>
    /// <returns>List IEvent  (список классов Событиеов)</returns>
    public async Task<IEnumerable<IEvent<Tmetadata>>> ConverterStreamResultToListEvent(IEnumerable<IEventJson> evnts)
    {
        return await Task.Run(() =>
        {
            var events = new List<IEvent<Tmetadata>>();
            foreach (var e in evnts)
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
    /// для преоброзования ResolvedEvent (класс в виде которого предсатвлен один Событие в Esdb)
    /// в IEvent (класс Событиеа)
    /// </summary>
    /// <param name="evnt">ResolvedEvent (класс в виде которого предсатвлен один Событие в Esdb)</param>
    /// <returns>IEvent (класс Событиеа)</returns>
    public abstract IEvent<Tmetadata> ConverterResolvedEventToEvent(IEventJson evnt);
}
