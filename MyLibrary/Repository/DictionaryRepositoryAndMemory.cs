using DictionaryManagment.Dictionary;
using DictionaryManagment.Model;

using Microsoft.EntityFrameworkCore;

namespace DictionaryManagment.Repository
{
    /// <summary>
    /// Репозиторий для словоря с хранением в памяти
    /// </summary>
    /// <typeparam name="DbC">Тип контекста</typeparam>
    /// <typeparam name="T">Тип модели</typeparam>
    /// <typeparam name="U">Тип ключа</typeparam>
    public class DictionaryRepositoryAndMemory<T, U> : DictionaryRepository<T, U> where T : class, IDictionaryModel<U> where U : struct
    {
        /// <summary>
        /// Словарь
        /// </summary>
        protected SafeDictionary<U, T> dictionary;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">БД Контекс</param>
        /// <param name="dictionary">Словарь</param>
        public DictionaryRepositoryAndMemory(DbContext context, SafeDictionary<U, T> dictionary) : base(context)
        {
            this.dictionary = dictionary;
        }

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="id">ключ</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public override T? Get(U id)
        {
            T? item = dictionary.Get(id);
            if (item == null)
            {
                item = base.Get(id);
                if (item != null)
                {
                    dictionary[item.Id] = item;
                }
            }
            return item;
        }

        /// <summary>
        /// Получение всех объектов с параметрами
        /// </summary>
        /// <returns>Список объектов</returns>
        public override IEnumerable<T> GetIEnumerable()
        {
            return dbSet.ToList();
        }


        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="id">ключ</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public virtual DictionaryModel<U>? GetShortValue(U id)
        {
            var item = Get(id);
            return item == null ? null : new DictionaryModel<U>()
            {
                Id = item.Id,
                Title = item.Title
            };
        }

        /// <summary>
        /// Получение всех объектов с параметрами
        /// </summary>
        /// <returns>Список объектов</returns>
        public virtual IEnumerable<DictionaryModel<U>> GetShortIEnumerable()
        {
            return dbSet.Select(m => new DictionaryModel<U>() { Id = m.Id, Title = m.Title }).ToList();
        }


        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="item">Объект</param>
        public override void Create(T item)
        {
            base.Create(item);
            int n = Save();
            if (n > 0)
            {
                dictionary.Set(item.Id,item);
            }
        }

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="item">Объект</param>
        public override void Update(T item)
        {
            base.Update(item);
            int n = Save();
            if (n > 0)
            {
                dictionary.Set(item.Id, item);
            }

        }
    }
}


