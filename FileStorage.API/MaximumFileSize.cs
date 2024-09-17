namespace FileStorage.Api;

/// <summary>
/// Максимальый размер файла
/// </summary>
public class MaximumFileSize
{
    /// <summary>
    /// Максимальый размер файла
    /// </summary>
    public long Value { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Максимальый размер файла</param>
    public MaximumFileSize(long value)
    {
        Value = value;
    }
}
