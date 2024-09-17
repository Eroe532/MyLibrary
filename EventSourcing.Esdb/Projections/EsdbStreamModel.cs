namespace EventSourcing.Esdb.Projections;

/// <summary>
/// Модель описания потоков Esdb
/// </summary>
public class EsdbStreamModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public EsdbStreamModel()
    {
        Title = "";
    }
}
