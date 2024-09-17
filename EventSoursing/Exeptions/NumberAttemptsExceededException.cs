namespace EventSoursing.Exeptions;

/// <summary>
/// Ошибка при получении события повторилась более 5 раз
/// </summary>
public class NumberAttemptsExceededException : Exception
{
    /// <summary>
    /// Количество попыток
    /// </summary>
    public static int Iterations { get; set; } = 5;

    /// <summary>
    /// Ошибка при записи события
    /// </summary>
    /// <param name="ex">Ошибка</param>
    /// <param name="position">Позиция в потоке</param>
    /// <param name="streamName">Имя потока</param>
    public NumberAttemptsExceededException(Exception ex, ulong position, string streamName) : base(ToMessage(ex, position, streamName), ex)
    {
    }

    /// <summary>
    /// Генерирование сообщения
    /// </summary>
    /// <param name="ex">Ошибка</param>
    /// <param name="position">Позиция в потоке</param>
    /// <param name="streamName">Имя потока</param>
    /// <returns></returns>
    private static string ToMessage(Exception ex, ulong position, string streamName)
    {
        return $"Ошибка при записи события: @{position}:{streamName} повторилась более {Iterations} раз" + Environment.NewLine + ex.Message;
    }
}
