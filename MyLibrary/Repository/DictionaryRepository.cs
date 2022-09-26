using DictionaryManagment.Model;
using Microsoft.EntityFrameworkCore;

namespace DictionaryManagment.Repository
{
    /// <summary>
    /// Репозиторий для словоря 
    /// </summary>
    /// <typeparam name="DbC">Тип контекста</typeparam>
    /// <typeparam name="T">Тип модели</typeparam>
    /// <typeparam name="U">Тип ключа</typeparam>
    public class DictionaryRepository<T, U> : IDictionaryManagmentReposytory<T, U> where T : class, IDictionaryModel<U> where U : struct
    {
        /// <summary>
        /// Контекст БД
        /// </summary>
        protected DbContext context;

        /// <summary>
        /// Таблица БД
        /// </summary>
        protected DbSet<T> dbSet;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст</param>
        public DictionaryRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        #region Dispose

        private bool disposed = false;

        /// <summary>
        /// Не вызывать
        ///
        /// Сводка:
        /// Releases the allocated resources for this context.
        ///
        /// Примечания:
        /// See DbContext lifetime, configuration, and initialization for more information.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Не вызывать
        ///
        /// Сводка:
        /// Releases the allocated resources for this context.
        ///
        /// Примечания:
        /// See DbContext lifetime, configuration, and initialization for more information.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="id">ключ</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public virtual T? Get(U id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Получение всех объектов с параметрами
        /// </summary>
        /// <returns>Список объектов</returns>
        public virtual IEnumerable<T> GetIEnumerable()
        {
            return dbSet;
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="item">Объект</param>
        public virtual void Create(T item)
        {
            dbSet.Add(item);
        }

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="item">Объект</param>
        public virtual void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            return context.SaveChanges();
        }

        /// <summary>
        /// Получение количества объектов в базе
        /// </summary>
        public int Count()
        {
            return dbSet.Count();
        }

    }
}


