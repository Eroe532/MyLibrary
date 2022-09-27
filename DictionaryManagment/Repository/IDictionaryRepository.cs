using DictionaryManagment.Model;

namespace DictionaryManagment.Repository
{
    /// <summary>
    /// Интерфейс для взаимодействия пользователя с репозиторием словаря
    /// </summary>
    /// <typeparam name="T">Тип модели</typeparam>
    /// <typeparam name="U">Тип ключа</typeparam>
    public interface IDictionaryRepository<T, U> : IDisposable where T : class, IDictionaryModel<U> where U : struct
    {
        /// <summary>
        /// Получение всех объектов с параметрами
        /// </summary>
        /// <returns>Список объектов</returns>
        IEnumerable<T> GetIEnumerable();

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        T? Get(U id);

    }
}


