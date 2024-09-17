namespace DictionaryManagment.Model;

/// <summary>
/// Интерфейс для модели с полем ключа
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
public class ShortModel<TKey> : IDictionaryModel<TKey> where TKey : struct
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
    public ShortModel()
    {
        Title = string.Empty;
    }
    /// <summary>
    /// Интерфейс для модели с полем ключа
    /// </summary>
    public ShortModel(IDictionaryModel<TKey> dictionaryModel)
    {
        Id = dictionaryModel.Id;
        Title = dictionaryModel.Title;
    }
}