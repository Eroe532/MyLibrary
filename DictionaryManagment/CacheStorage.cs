using System.Linq.Expressions;

using DictionaryManagment.Dictionary;
using DictionaryManagment.Model;

namespace DictionaryManagment;

/// <summary>Кэш Хранилище</summary>
public abstract class CacheStorage : ICacheStorage
{
    /// <summary>Словари</summary>
    protected readonly Dictionary<Type, object> _dictionaries = new();

    /// <summary>Изменения</summary>
    protected readonly Dictionary<Type, object> _changes = new();

    /// <inheritdoc/>
    public Dictionary<TKey, TValue> GetDictionary<TKey, TValue>() where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        return GetSafeDictionary<TKey, TValue>().ToDictionary();
    }

    /// <inheritdoc/>
    public SafeDictionary<TKey, TValue> GetSafeDictionary<TKey, TValue>() where TKey : struct where TValue : class, IDictionaryModel<TKey>
    {
        return (SafeDictionary<TKey, TValue>)_dictionaries[typeof(TValue)];
    }

    /// <inheritdoc/>
    public TValue? GetItemDictionary<TKey, TValue>(TKey? id)
        where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        if (!id.HasValue)
            return null;
        return GetItemDictionary<TKey, TValue>(id.Value);
    }

    /// <inheritdoc/>
    public TValue GetItemDictionary<TKey, TValue>(TKey id)
        where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        return GetSafeDictionary<TKey, TValue>().TryGetValue(id, out var result)
            ? result
            : throw new NullReferenceException("Нет такого значения в словаре");
    }

    /// <summary>Добавить словарь</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="dictionary">Словарь</param>
    /// <returns></returns>
    protected bool AddDictionary<TKey, TValue>(SafeDictionary<TKey, TValue> dictionary) where TKey : struct where TValue : class, IDictionaryModel<TKey>
    {
        return _dictionaries.TryAdd(typeof(TValue), dictionary);
    }

    /// <inheritdoc/>
    public SafeDictionary<TKey, TValue> GetChanges<TKey, TValue>() where TKey : struct where TValue : class, IDictionaryModel<TKey>
    {
        if (!_changes.TryGetValue(typeof(TValue), out var dictionary))
        {
            dictionary = new SafeDictionary<TKey, TValue>();
            _changes.Add(typeof(TValue), dictionary);
        }
        return (SafeDictionary<TKey, TValue>)dictionary;
    }

    /// <summary>Внести изменение в словарь</summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    private void AddChanges<TKey, TValue>(TValue value, TypeChangeEnum typeChange) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        if (!_changes.TryGetValue(typeof(TValue), out var list))
        {
            list = new List<Change<TKey, TValue>>();
            _changes.Add(typeof(TValue), list);
        }
        ((List<Change<TKey, TValue>>)list).Add(new() { Value = value, TypeChange = typeChange });
    }

    /// <inheritdoc/>
    public void AddValue<TKey, TValue>(TValue value) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        AddChanges<TKey, TValue>(value, TypeChangeEnum.Add);
    }

    /// <inheritdoc/>
    public void UpdateValue<TKey, TValue>(TValue value) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        AddChanges<TKey, TValue>(value, TypeChangeEnum.Update);
    }

    /// <inheritdoc/>
    public void DeleteValue<TKey, TValue>(TValue value) where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        AddChanges<TKey, TValue>(value, TypeChangeEnum.Delete);
    }

    /// <inheritdoc/>
    public int SaveChange()
    {
        var res = 0;
        if (_changes.Count > 0)
        {
            foreach (var pair in _changes)
            {
                foreach (var change in (dynamic)pair.Value)
                {
                    var item = change.Value;
                    switch (change.TypeChange)
                    {
                        case TypeChangeEnum.Add:
                            {
                                if (((dynamic)_dictionaries[pair.Key]).TryAdd(item.Id, item))
                                {
                                    res++;
                                }
                                break;
                            }
                        case TypeChangeEnum.Update:
                            {
                                ((dynamic)_dictionaries[pair.Key]).Remove(item.Id);
                                if (((dynamic)_dictionaries[pair.Key]).TryAdd(item.Id, item))
                                {
                                    res++;
                                }
                                break;
                            }
                        case TypeChangeEnum.Delete:
                            {
                                ((dynamic)_dictionaries[pair.Key]).Remove(item.Id);
                                res++;
                                break;
                            }

                        default:
                            break;
                    }


                }
            }
        }
        return res;
    }

    /// <inheritdoc/>
    public void DeleteChange()
    {
        _changes.Clear();
    }

    private enum TypeChangeEnum
    {
        Add,
        Update,
        Delete
    }

    private class Change<TKey, TValue> where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        public TValue Value { get; set; }
        public TypeChangeEnum TypeChange { get; set; }

        public Change()
        {
            Value = new();
        }
    }
}