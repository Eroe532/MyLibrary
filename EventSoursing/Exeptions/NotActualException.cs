namespace EventSoursing.Exeptions;

/// <summary>
/// Ошибка Не удалось получить актуальные данные
/// </summary>
public class NotActualException : Exception
{
    /// <summary>
    /// Ошибка Не удалось получить актуальные данные
    /// </summary>
    public NotActualException() : base(ToMessage())
    {
    }


    /// <summary>
    /// Ошибка Не удалось получить актуальные данные
    /// </summary>
    public NotActualException(Exception innerException) : base(ToMessage(), innerException)
    {
    }

    /// <summary>
    /// Генерирование сообщения
    /// </summary>
    /// <returns></returns>
    private static string ToMessage()
    {
        return $"Не удалось получить актуальные данные";
    }
}
