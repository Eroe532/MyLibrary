using System.Collections.Concurrent;

using DictionaryManagment.Model;

namespace DictionaryManagment.Dictionary;

/// <summary>
/// Словарь
/// </summary>
/// <typeparam name="TKey">Ключ</typeparam>
/// <typeparam name="TValue">Значение</typeparam>
public class SafeDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>/*, ISafeDictionary<TKey, TValue>*/
    where TValue : class, IDictionaryModel<TKey>
    where TKey : struct
{
    /// <summary>
    /// Словарь
    /// </summary>
    /// <param name="dictionary"> Словарь </param>
    public SafeDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
    {
    }

    /// <summary>
    /// Словарь
    /// </summary>
    internal SafeDictionary() : base()
    {
    }

    /// <summary>
    /// Взаимодействие по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns></returns>
    public new TValue this[TKey key]
    {
        get => TryGetValue(key, out var value) ? value : throw new Exception(); //todo 
        set => AddOrUpdate(key, factory => value, (keyValue, oldValue) => value);
    }

    internal void Remove(TKey key)
    {
        TryRemove(key, out _);
    }

    /// <summary>
    /// неявное преобразование
    /// </summary>
    /// <param name="input"></param>
    public static implicit operator SafeDictionary<TKey, IDictionaryModel<TKey>>(SafeDictionary<TKey, TValue> input)
    {
        return new SafeDictionary<TKey, IDictionaryModel<TKey>>(input.ToDictionary(k => k.Key, v => (IDictionaryModel<TKey>)v.Value));
    }

    /// <summary>
    /// Преобразование к Dictionary
    /// </summary>
    /// <returns></returns>
    public Dictionary<TKey, TValue> ToDictionary()
    {
        return this.ToDictionary(k => k.Key, v => v.Value);
    }


    /// <summary>
    /// Преобразование к Dictionary
    /// </summary>
    /// <returns></returns>
    public Dictionary<TKey, ShortModel<TKey>> ToShortModelDictionary()
    {
        return this.ToDictionary(k => k.Key, v => new ShortModel<TKey>(v.Value));
    }
}