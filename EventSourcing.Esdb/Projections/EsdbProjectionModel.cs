namespace EventSourcing.Esdb.Projections;

/// <summary>
/// Класс проекции
/// </summary>
public class EsdbProjectionModel
{
    /// <summary>
    /// Конструктор
    /// </summary>
    public EsdbProjectionModel()
    {
        NameProjection = "";
        JsBodyProjection = "";
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название проекции
    /// </summary>
    public string NameProjection { get; set; }

    /// <summary>
    /// Код проекции
    /// </summary>
    public string JsBodyProjection { get; set; }
}

