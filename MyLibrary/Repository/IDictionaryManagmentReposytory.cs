using DictionaryManagment.Model;

namespace DictionaryManagment.Repository
{
    /// <summary>
    /// Интерфейс для взаимодействия Администратора словаря с репозиторием 
    /// </summary>
    /// <typeparam name="T">Тип модели</typeparam>
    /// <typeparam name="U">Тип ключа</typeparam>
    public interface IDictionaryManagmentReposytory<T, U> : IDictionaryRepository<T, U> where T : class, IDictionaryModel<U> where U : struct
    {
        /// <summary>
        /// Добавление Объекта в базу данных
        /// </summary>
        /// <param name="item">Объект</param>
        void Create(T item);

        /// <summary>
        /// Сохранение
        /// </summary>
        int Save();
    }
}


