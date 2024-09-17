using System.Text;

using EventStore.Client;

using Microsoft.Extensions.Logging;

namespace EventSourcing.Esdb.Projections;

/// <summary>
/// Класс для управления проекциями
/// </summary>
public abstract class ProjectionManagement
{
    /// <summary>
    /// Клиент управления проекциями EventStoreDB
    /// </summary>
    protected EventStoreProjectionManagementClient _managementClient;

    /// <summary>
    /// Клиент EventStoreDB
    /// </summary>
    protected EventStoreClient _client;

    /// <summary>
    /// Логгер
    /// </summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// Конфигурации лучше загружать из json или других типов файлов
    /// </summary>
    protected EsdbConfModel? eventStoreConf;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="eventConnectionString">Строка подключения </param>
    /// <param name="client">Клиент EventStoreDB</param>
    /// <param name="eventStoreConf">Конфигурации лучше загружать из json или других типов файлов</param>
    /// <param name="logger">Логгер</param>
    public ProjectionManagement(string eventConnectionString, EventStoreClient client, EsdbConfModel? eventStoreConf, ILogger logger)
    {
        _managementClient = new EventStoreProjectionManagementClient(EventStoreClientSettings.Create(eventConnectionString));
        _client = client;
        this.eventStoreConf = eventStoreConf;
        _logger = logger;
    }

    /// <summary>
    /// Проверка наличия проекции или обновление его
    /// </summary>
    /// <param name="NameProjection">Имя проекции</param>
    /// <param name="jsBodyProjection">Код проекции</param>
    /// <param name="isChange">Требуется ли обновить его или нет</param>
    /// <returns></returns>
    protected async Task CheckProjectionAsync(string NameProjection, string jsBodyProjection, bool isChange)
    {
        var details = _managementClient.ListAllAsync();
        try
        {
            if (!await details.AnyAsync(p => p.Name == NameProjection))
            {

                await _managementClient.CreateContinuousAsync(NameProjection, jsBodyProjection, true);

            }
            else if (isChange)
            {
                await _managementClient.UpdateAsync(NameProjection, jsBodyProjection, true);
                await _managementClient.ResetAsync(NameProjection);
            }
        }
        catch (InvalidOperationException e) when (e.Message.Contains("Conflict"))
        {
            _logger.LogError(e, $"{NameProjection} already exists");
        }
    }
    /// <summary>
    /// Проверка существования всех проекций или обновление их  
    /// </summary>
    /// <param name="listProjection">Список проекций</param>
    /// <param name="isChange">Требуется ли обновить их</param>
    /// <returns></returns>
    protected async Task CheckAllProjectionAsync(List<EsdbProjectionModel> listProjection, bool isChange = true)
    {
        try
        {
            var details = _managementClient.ListAllAsync();
            await foreach (var item in details)
            {
                await _managementClient.DisableAsync(item.Name);
            }

            foreach (var projections in listProjection)
            {
                if (!await details.AnyAsync(p => p.Name == projections.NameProjection))
                {
                    await _managementClient.CreateContinuousAsync(projections.NameProjection, projections.JsBodyProjection, true);
                }
                else if (isChange)
                {
                    await _managementClient.UpdateAsync(projections.NameProjection, projections.JsBodyProjection, true);
                    await _managementClient.ResetAsync(projections.NameProjection);
                }

                await StartProjectionAsync(projections.NameProjection);
            }
        }
        catch (InvalidOperationException e) when (e.Message.Contains("Conflict"))
        {
            _logger.LogError(e, $"AllProjection already exists");
        }
    }

    /// <summary>
    /// Получение состояния проекции
    /// </summary>
    /// <param name="NameProjection">Имя проекции</param>
    /// <returns></returns>
    protected async Task<ProjectionDetails?> GetStatusAsync(string NameProjection)
    {
        return await _managementClient.GetStatusAsync(NameProjection);
    }

    /// <summary>
    /// Запуск проекции
    /// </summary>
    /// <param name="NameProjection">Имя проекции</param>
    /// <returns></returns>
    protected async Task<ProjectionDetails?> StartProjectionAsync(string NameProjection)
    {
        try
        {
            var state = await GetStatusAsync(NameProjection);
            if (state != null && state.Status != "Running")
            {
                await _managementClient.EnableAsync(NameProjection);
                return await GetStatusAsync(NameProjection);
            }

            return state;
        }
        catch (InvalidOperationException e) when (e.Message.Contains("NotFound"))
        {
            _logger.LogError(e, $"{NameProjection} already exists");
            return null;
        }
    }

    /// <summary>
    /// Проверка наличия потока и создание в случае отсутствия
    /// </summary>
    /// <param name="stream">имя потока</param>
    /// <returns></returns>
    protected async Task CheckStreamAsync(EsdbStreamModel stream)
    {
        var result = _client.ReadStreamAsync(
            Direction.Forwards,
            stream.Title,
            revision: 10,
            maxCount: 20);

        if (await result.ReadState == ReadState.StreamNotFound)
        {
            await _client.AppendToStreamAsync(
            stream.Title,
                StreamState.Any,
                new List<EventData>() { new EventData(Uuid.FromGuid(Guid.NewGuid()), "CreateStream", Encoding.UTF8.GetBytes("{ }"), Encoding.UTF8.GetBytes("{ }")) });
        }
    }

    /// <summary>
    /// Проверка наличия потоков и создание в случае отсутствия
    /// </summary>
    /// <param name="listStream">Список имен потоков</param>
    /// <returns></returns>
    protected async Task CheckStreamAsync(IEnumerable<EsdbStreamModel> listStream)
    {
        foreach (var stream in listStream)
        {
            await CheckStreamAsync(stream);
        }
    }
}


