using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventSourcing.DependencyInjection;

namespace EventSourcing.Esdb.Extensions;

/// <summary>
/// Информация о потоках
/// </summary>
public class StreamInfo : IStreamInfo
{
    /// <inheritdoc/>
    public Dictionary<int, string> Streams { get; }

    /// <summary>
    /// Информация о потоках
    /// </summary>
    public StreamInfo()
    {
        Streams = new();
    }

    /// <inheritdoc/>
    public string GetStreamName(int streamId)
    {
        return Streams[streamId];
    }

    /// <inheritdoc/>
    public virtual string GetProjectionName(int streamId, string projectionPart)
    {
        return $"{GetStreamName(streamId)}-{projectionPart}";
    }
    /// <summary>
    /// Добавить поток 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="name">Название</param>
    protected void AddStream(int id, string name)
    {
        Streams.Add(id, name);
    }
}
