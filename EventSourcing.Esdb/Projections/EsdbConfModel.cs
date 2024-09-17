namespace EventSourcing.Esdb.Projections;

/// <summary>
/// Класс конфигураций для соотношения его с json файлом
/// </summary>
public class EsdbConfModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Были ли изменения (true изменения применятся и выставится false)
    /// </summary>
    public bool IsChange { get; set; }

    /// <summary>
    /// Дата применения
    /// </summary>
    public DateTime ChangeDate { get; set; }

    /// <summary>
    /// Список проекций
    /// </summary>
    public List<EsdbProjectionModel> ListProjection { get; set; } = new List<EsdbProjectionModel>();

    /// <summary>
    /// Список потоков
    /// </summary>
    public List<EsdbStreamModel> ListStream { get; set; } = new List<EsdbStreamModel>();
}


