using DictionaryManagment;
using DictionaryManagment.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DictionaryManagment.Manager;

/// <summary>
/// Репозиторий для словаря с хранением в памяти
/// </summary>
public class DictionaryManager
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    protected readonly DbContext _context;

    /// <summary>
    /// Кэш хранилище
    /// </summary>
    protected readonly ICacheStorage _cacheStorage;

    /// <summary>
    /// Запущена ли транзакция
    /// </summary>
    protected bool IsTransaction { get; private set; }

    /// <summary>
    /// Количество сохраняемых объектов в кэш
    /// </summary>
    protected int CountSaveInCache { get; private set; }

    /// <summary>
    /// Репозиторий для словаря с хранением в памяти
    /// </summary>
    /// <param name="context">Контекст БД</param>
    /// <param name="cacheStorage">Кэш хранилище</param>
    public DictionaryManager(DbContext context, ICacheStorage cacheStorage)
    {
        IsTransaction = false;
        CountSaveInCache = 0;
        _context = context;
        _cacheStorage = cacheStorage;
    }
    #region Transaction

    /// <inheritdoc/>
    public IDbContextTransaction BeginTransaction()
    {
        var res = _context.Database.BeginTransaction();
        IsTransaction = true;
        if (CountSaveInCache > 0)
        {
            _cacheStorage.DeleteChange();
            CountSaveInCache = 0;
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var res = await _context.Database.BeginTransactionAsync(cancellationToken);
        IsTransaction = true;
        if (CountSaveInCache > 0)
        {
            _cacheStorage.DeleteChange();
            CountSaveInCache = 0;
        }
        return res;
    }

    /// <inheritdoc/>
    public void CommitTransaction()
    {
        _context.Database.CommitTransaction();
        if (IsTransaction && CountSaveInCache > 0)
        {
            _cacheStorage.SaveChange();
            IsTransaction = false;
            CountSaveInCache = 0;
        }
    }

    /// <inheritdoc/>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
        if (IsTransaction && CountSaveInCache > 0)
        {
            _cacheStorage.SaveChange();
            IsTransaction = false;
            CountSaveInCache = 0;
        }
    }

    /// <inheritdoc/>
    public void RollbackTransaction()
    {
        _context.Database.RollbackTransaction();
        if (IsTransaction && CountSaveInCache > 0)
        {
            _cacheStorage.DeleteChange();
            IsTransaction = false;
            CountSaveInCache = 0;
        }
    }

    /// <inheritdoc/>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
        if (IsTransaction && CountSaveInCache > 0)
        {
            _cacheStorage.DeleteChange();
            IsTransaction = false;
            CountSaveInCache = 0;
        }
    }


    #endregion

    /// <inheritdoc/>
    public virtual TValue? GetFromCache<TKey, TValue>(Func<TValue, bool> predicate) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        return _cacheStorage.GetDictionary<TKey, TValue>().Values.SingleOrDefault(predicate);
    }

    /// <inheritdoc/>
    public virtual TValue? GetFromCache<TKey, TValue>(TKey id) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        return _cacheStorage.GetDictionary<TKey, TValue>().TryGetValue(id, out var item) ? item : null;
    }

    /// <inheritdoc/>
    public virtual TValue? GetFromCache<TValue>(Guid id) where TValue : class, IDictionaryModel<Guid>, new()
    {
        return GetFromCache<Guid, TValue>(id);
    }

    /// <inheritdoc/>
    public virtual TValue? GetFromCache<TValue>(int id) where TValue : class, IDictionaryModel<int>, new()
    {
        return GetFromCache<int, TValue>(id);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TKey, TValue>() where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        return _cacheStorage.GetDictionary<TKey, TValue>().Values;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TKey, TValue>(IEnumerable<TKey> ids) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        var result = new List<TValue>();
        foreach (var id in ids)
        {
            var item = GetFromCache<TKey, TValue>(id);
            if (item != null)
            {
                result.Add(item);
            }
        }
        return result;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TValue>(IEnumerable<Guid> ids) where TValue : class, IDictionaryModel<Guid>, new()
    {
        return GetIEnumerableFromCache<Guid, TValue>(ids);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TValue>(IEnumerable<int> ids) where TValue : class, IDictionaryModel<int>, new()
    {
        return GetIEnumerableFromCache<int, TValue>(ids);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TKey, TValue>(Func<TValue, bool> predicate) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        return _cacheStorage.GetDictionary<TKey, TValue>().Values.Where(predicate);
    }

    /// <inheritdoc/>
    public Dictionary<TKey, TValue> GetDictionaryFromCache<TKey, TValue>() where TKey : struct where TValue : class, IDictionaryModel<TKey>, new()
    {
        return _cacheStorage.GetDictionary<TKey, TValue>().ToDictionary(k => k.Key, v => v.Value);
    }

    /// <inheritdoc/>
    public virtual TResult? MaxFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        var collection = _cacheStorage.GetDictionary<TKey, TValue>().Values;
        return collection.Any() ? collection.Max(selector) : default;
    }

    /// <inheritdoc/>
    public virtual TValue? MaxByFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        var collection = _cacheStorage.GetDictionary<TKey, TValue>().Values;
        return collection.Any() ? collection.MaxBy(selector) : null;
    }

    /// <inheritdoc/>
    public virtual TResult? MinFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        var collection = _cacheStorage.GetDictionary<TKey, TValue>().Values;
        return collection.Any() ? collection.Min(selector) : default;
    }

    /// <inheritdoc/>
    public virtual TValue? MinByFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        var collection = _cacheStorage.GetDictionary<TKey, TValue>().Values;
        return collection.Any() ? collection.MinBy(selector) : null;
    }

    /// <inheritdoc/>
    public virtual void CreateInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        AutoIncrement<TKey, TValue>(item);
        _context.Set<TValue>().Add(item);
        _cacheStorage.AddValue<TKey, TValue>(item);
        CountSaveInCache++;
    }

    private void AutoIncrement<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        if (item is IDictionaryModel<Guid> intItem)
        {
            if (intItem.Id == default)
            {
                intItem.Id = Guid.NewGuid();
            }
        }
    }

    /// <inheritdoc/>
    public virtual void UpdateInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        _context.Set<TValue>().Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        _cacheStorage.UpdateValue<TKey, TValue>(item);
        CountSaveInCache++;
    }

    /// <inheritdoc/>
    public virtual void PhysicalDeleteInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct
    {
        var dbSet = _context.Set<TValue>();
        dbSet.Attach(item);
        dbSet.Remove(item);
        _cacheStorage.DeleteValue<TKey, TValue>(item);
        CountSaveInCache++;
    }

    /// <inheritdoc/>
    public int SaveChanges()
    {
        var res = _context.SaveChanges();
        if (!IsTransaction && res > 0 && CountSaveInCache > 0)
        {
            _cacheStorage.SaveChange();
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        var res = await _context.SaveChangesAsync();
        ;
        if (!IsTransaction && res > 0 && CountSaveInCache > 0)
        {
            _cacheStorage.SaveChange();
        }
        return res;
    }
}
