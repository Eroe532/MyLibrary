using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventSourcing.Converter;
using EventSourcing.DependencyInjection;
using EventSourcing.Esdb.Producer;
using EventSourcing.Esdb.Reader;
using EventSourcing.Events;
using EventSourcing.Producer;
using EventSourcing.Reader;

using EventStore.Client;

namespace EventSourcing.Esdb.Extensions;

/// <summary>
/// Фабрика Репозиториев EventStoreDB
/// </summary>
/// <typeparam name="TMetadata"></typeparam>
public class EsdbFactory<TMetadata> : IEventSourcingFactory<TMetadata> where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
{
    ///<inheritdoc/>
    public IStreamInfo StreamInfo { get; }

    /// <summary>
    /// Клиент EventStoreDB
    /// </summary>
    private readonly EventStoreClient _client;

    ///<inheritdoc/>
    public IEventConverter<TMetadata> EventConverter { get; }


    /// <summary>
    /// Фабрика Репозиториев EventStoreDB
    /// </summary>
    /// <param name="streamInfo">Информация о потоках</param>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="eventConverter">Класс для преобразования в ивенты</param>
    public EsdbFactory(IStreamInfo streamInfo, EventStoreClient client, IEventConverter<TMetadata> eventConverter)
    {
        StreamInfo = streamInfo;
        _client = client;
        EventConverter = eventConverter;
    }


    /// <summary>
    /// Получение продюсера
    /// </summary>
    /// <param name="streamId">Идентификатор потока</param>
    /// <returns></returns>
    public IESProducer<TMetadata> GetProducer(int streamId)
    {
        return new EsdbProducer<TMetadata>(_client, StreamInfo.GetStreamName(streamId));
    }

    /// <summary>
    /// Получение ридера
    /// </summary>
    /// <param name="streamId">Идентификатор потока</param>
    /// <returns></returns>
    public IESReader<TMetadata> GetReader(int streamId)
    {
        return new EsdbReader<TMetadata>(_client, StreamInfo.GetStreamName(streamId), EventConverter);
    }

    /// <summary>
    /// Получение ридера для проекций
    /// </summary>
    /// <param name="streamId">Идентификатор потока</param>
    /// <param name="projectionPart">Идентификатор проекции</param>
    /// <returns></returns>
    public IESReader<TMetadata> GetProjectionReader(int streamId, string projectionPart)
    {
        return new EsdbReader<TMetadata>(_client, StreamInfo.GetProjectionName(streamId, projectionPart), EventConverter);
    }
}
