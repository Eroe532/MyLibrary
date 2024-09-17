using DictionaryManagment.Model;

using Microsoft.EntityFrameworkCore.Storage;

namespace DictionaryManagment.Manager;

/// <summary>
/// Репозиторий для словаря с хранением в памяти
/// </summary>
public interface IDictionaryManager
{
    #region Transaction

    /// <inheritdoc/>
    public IDbContextTransaction BeginTransaction();

    /// <inheritdoc/>
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public void CommitTransaction();

    /// <inheritdoc/>
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public void RollbackTransaction();

    /// <inheritdoc/>
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);


    #endregion

    /// <summary>
    /// Получение из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="predicate">Предикат</param>
    /// <returns></returns>
    protected TValue? GetFromCache<TKey, TValue>(Func<TValue, bool> predicate) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Получение из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public TValue? GetFromCache<TKey, TValue>(TKey id) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Получение из Кэша
    /// </summary>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public TValue? GetFromCache<TValue>(Guid id) where TValue : class, IDictionaryModel<Guid>, new();

    /// <summary>
    /// Получение из Кэша
    /// </summary>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="id">Идентификатор</param>
    /// <returns></returns>
    public TValue? GetFromCache<TValue>(int id) where TValue : class, IDictionaryModel<int>, new();

    /// <summary>
    /// Получение коллекции из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    public IEnumerable<TValue> GetIEnumerableFromCache<TKey, TValue>() where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Получение коллекции из Кэша
    /// </summary>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="ids">Коллекция идентификаторов</param>
    /// <returns></returns>
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

    /// <summary>
    /// Получение коллекции из Кэша
    /// </summary>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="ids">Коллекция идентификаторов</param>
    /// <returns></returns>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TValue>(IEnumerable<Guid> ids) where TValue : class, IDictionaryModel<Guid>, new()
    {
        return GetIEnumerableFromCache<Guid, TValue>(ids);
    }

    /// <summary>
    /// Получение коллекции из Кэша
    /// </summary>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="ids">Коллекция идентификаторов</param>
    /// <returns></returns>
    public virtual IEnumerable<TValue> GetIEnumerableFromCache<TValue>(IEnumerable<int> ids) where TValue : class, IDictionaryModel<int>, new()
    {
        return GetIEnumerableFromCache<int, TValue>(ids);
    }

    /// <summary>
    /// Получение коллекции из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="predicate">Предикат</param>
    /// <returns></returns>
    public IEnumerable<TValue> GetIEnumerableFromCache<TKey, TValue>(Func<TValue, bool> predicate) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;


    /// <summary>
    /// Получение словаря из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <returns></returns>
    public Dictionary<TKey, TValue> GetDictionaryFromCache<TKey, TValue>() where TKey : struct where TValue : class, IDictionaryModel<TKey>, new();

    /// <summary>
    /// Получение максимального значения из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <typeparam name="TResult">Тип поля или свойства</typeparam>
    /// <param name="selector">Выбор свойства или поля</param>
    /// <param name="predicate">Предикат</param>
    /// <returns></returns>
    public TResult? MaxFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Получение объекта с максимальным значением из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <typeparam name="TResult">Тип поля или свойства</typeparam>
    /// <param name="selector">Выбор свойства или поля</param>
    /// <param name="predicate">Предикат</param>
    /// <returns></returns>
    public TValue? MaxByFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Получение минимального значения из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <typeparam name="TResult">Тип поля или свойства</typeparam>
    /// <param name="selector">Выбор свойства или поля</param>
    /// <param name="predicate">Предикат</param>
    /// <returns></returns>
    public TResult? MinFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Получение объекта с минимальным значением из Кэша
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <typeparam name="TResult">Тип поля или свойства</typeparam>
    /// <param name="selector">Выбор свойства или поля</param>
    /// <param name="predicate">Предикат</param>
    /// <returns></returns>
    public TValue? MinByFromCache<TKey, TValue, TResult>(Func<TValue, TResult> selector, Func<TValue, bool>? predicate = null) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Создание объекта в БД с записью в кэш
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="item"></param>
    public void CreateInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Обновление объекта в БД и в кэше
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="item"></param>
    public void UpdateInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Обновление объекта в БД и в кэше
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="item"></param>
    public void DeleteInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;

    /// <summary>
    /// Обновление объекта в БД и в кэше
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип модели</typeparam>
    /// <param name="item"></param>
    public void PhysicalDeleteInCache<TKey, TValue>(TValue item) where TValue : class, IDictionaryModel<TKey>, new() where TKey : struct;


    /// <inheritdoc/>
    public int SaveChanges();

    /// <inheritdoc/>
    public Task<int> SaveChangesAsync();
}