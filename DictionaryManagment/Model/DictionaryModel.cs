namespace DictionaryManagment.Model;

/// <summary>
/// Интерфейс для модели с полем ключа
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
public class DictionaryModel<TKey> : IDictionaryModel<TKey> where TKey : struct
{
    /// <summary>
    /// Ключ
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Интерфейс для модели с полем ключа
    /// </summary>
    public DictionaryModel()
    {
        Title = string.Empty;
    }
}
