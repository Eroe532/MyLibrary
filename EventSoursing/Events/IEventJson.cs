using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSoursing.Events;

/// <summary>
/// Событие в Json
/// </summary>
public interface IEventJson
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Данные
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// Метаданные
    /// </summary>
    public byte[] Metadata { get; set; }

    /// <summary>
    /// Позицыя
    /// </summary>
    public ulong Position { get; set; }

    /// <summary>
    /// Время создания
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// Тип события
    /// </summary>
    public string EventType { get; set; }
}
