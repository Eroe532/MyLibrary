using System.Security.Cryptography.X509Certificates;

namespace DictionaryManagment.Model;

/// <summary>
/// Интерфейс для модели с полем ключа
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
public interface IDictionaryModel<TKey> where TKey : struct
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }
}