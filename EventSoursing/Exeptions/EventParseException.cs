namespace EventSoursing.Exeptions;

/// <summary>
/// Ошибка при получении события
/// </summary>
public class EventParseException : Exception
{
    /// <summary>
    /// Ошибка при получении события
    /// </summary>
    /// <param name="type">Тип события</param>
    public EventParseException(Type type) : base(ToMessage(type))
    {
    }

    /// <summary>
    /// Ошибка при получении события
    /// </summary>
    /// <param name="type">Тип события</param>
    public EventParseException(string type) : base(ToMessage(type))
    {
    }

    /// <summary>
    /// Генерирование сообщения
    /// </summary>
    /// <param name="type">Тип события</param>
    /// <returns></returns>
    private static string ToMessage(Type type)
    {
        return $"Ошибка при получении события {type}";
    }

    /// <summary>
    /// Генерирование сообщения
    /// </summary>
    /// <param name="type">Тип события</param>
    /// <returns></returns>
    private static string ToMessage(string type)
    {
        return $"Ошибка при получении события {type}";
    }
}
