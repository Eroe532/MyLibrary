using EventSourcing.Esdb.Events;
using EventSourcing.Events;
using EventSourcing.Reader;

using EventStore.Client;

namespace EventSourcing.Esdb.Extensions;

/// <summary>
/// Расширения конвертации
/// </summary>
public static class EsdbExtension
{
    /// <summary>
    /// Преобразование класса к классу T
    /// </summary>
    /// <typeparam name="TMetadata"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    public static EventData EventData<TMetadata>(this IEvent<TMetadata> @event) where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
    {
        return new EventData(
            Uuid.FromGuid(@event.EventId),
            @event.Metadata.EventTypeName,
            @event.JsonSerializeData(),
            @event.JsonSerializeMetadata()
        );
    }

    /// <summary>
    /// Конвертация
    /// </summary>
    /// <param name="direction">Режимов чтения</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static Direction Convert(this ESDirection direction)
    {
        return direction switch
        {
            ESDirection.Backwards => Direction.Backwards,
            ESDirection.Forwards => Direction.Forwards,
            _ => throw new NotSupportedException()
        };
    }

    /// <summary>
    /// Конвертация
    /// </summary>
    /// <param name="resolvedEvent">Прочитанное событие из EventStore</param>
    /// <returns></returns>
    public static IEventJson Convert(this ResolvedEvent resolvedEvent)
    {
        return (EventJson)resolvedEvent;
    }
}
