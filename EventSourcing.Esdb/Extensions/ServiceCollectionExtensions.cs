using CaTConfiguration;

using EventSourcing.Converter;
using EventSourcing.DependencyInjection;
using EventSourcing.Events;
using EventSourcing.Subscriber;

using EventStore.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace EventSourcing.Esdb.Extensions;

/// <summary>
/// Расширение
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация подписчика
    /// </summary>
    public static IServiceCollection AddHostedSubscriber<TESSubscriber>(this IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Singleton) where TESSubscriber : class, IESSubscriber, IHostedService
    {
        serviceCollection.AddHostedService<TESSubscriber>();
        switch (lifetime) 
            {
        case ServiceLifetime.Singleton:
                {
                    serviceCollection.AddKeyedSingleton<IESSubscriber>(typeof(IESSubscriber).Name,(x,_) => x.GetServices<IHostedService>().OfType<TESSubscriber>().First());
                    break;
                }
                case ServiceLifetime.Transient:
                {
                    serviceCollection.AddKeyedTransient<IESSubscriber>(typeof(IESSubscriber).Name, (x, _) => x.GetServices<IHostedService>().OfType<TESSubscriber>().First());
                    break;
                }
            case ServiceLifetime.Scoped:
                {
                    serviceCollection.AddKeyedScoped<IESSubscriber>(typeof(IESSubscriber).Name, (x, _) => x.GetServices<IHostedService>().OfType<TESSubscriber>().First());
                    break;
                }
            default:
                {
                    break;
                }
        }
        
        return serviceCollection;
    }

    /// <summary>
    /// Регистрация Фабрики и
    /// </summary>
    /// <typeparam name="TMetadata"></typeparam>
    /// <typeparam name="TStreamInfo"></typeparam>
    /// <param name="serviceCollection">ServiceCollection</param>
    /// <param name="connectionString">Строка подключения</param>
    /// <param name="configureSettings"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventStoreDB<TMetadata, TStreamInfo>(this IServiceCollection serviceCollection, string connectionString = "EventStoreProvider", Action<EventStoreClientSettings>? configureSettings = null, IConfiguration? configuration = null) 
        where TMetadata : IEventMetadata, ICloneMetadata<TMetadata>, new()
        where TStreamInfo :class, IStreamInfo, new()
    {
        configuration ??= CaTConfig.GetIConfiguration();
        serviceCollection.TryAddSingleton<IStreamInfo, TStreamInfo>();
        serviceCollection.AddEventStoreClient(configuration.GetConnectionString(connectionString)?? connectionString, configureSettings);
        serviceCollection.TryAddSingleton<IEventSourcingFactory<TMetadata>, EsdbFactory<TMetadata>>();
        return serviceCollection;
    }
}
