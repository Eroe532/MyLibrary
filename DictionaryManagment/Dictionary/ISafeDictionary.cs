using DictionaryManagment.Model;

namespace DictionaryManagment.Dictionary
{
    /// <summary>
    /// Инерфейс для словаря
    /// </summary>
    /// <typeparam name="TKey">Ключ</typeparam>
    /// <typeparam name="TValue">Значение</typeparam>
    public interface ISafeDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>,  IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue> where TKey : struct where TValue : class, IDictionaryModel<TKey>
    {

    }

}