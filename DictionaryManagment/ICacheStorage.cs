using System.Linq.Expressions;

using DictionaryManagment.Model;

namespace DictionaryManagment;

/// <summary>
/// Кэш Хранилище
/// </summary>
public interface ICacheStorage
{
    /// <summary>Получить словарь</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    public Dictionary<TKey, TValue> GetDictionary<TKey, TValue>() where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>Получение строки словаря T с ключём TKey равному id nullable</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public TValue? GetItemDictionary<TKey, TValue>(TKey? id)
        where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>Получение строки словаря T с ключём TKey равному id</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public TValue GetItemDictionary<TKey, TValue>(TKey id)
        where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>Добавить значение</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    public void AddValue<TKey, TValue>(TValue value) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>Изменить значение</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    public void UpdateValue<TKey, TValue>(TValue value) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>Удалить значение</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    public void DeleteValue<TKey, TValue>(TValue value) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>Сохранение изменений</summary>
    /// <returns></returns>
    public int SaveChange();

    /// <summary>Откат изменений</summary>
    public void DeleteChange();

}
