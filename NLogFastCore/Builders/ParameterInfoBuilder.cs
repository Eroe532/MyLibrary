using System.Reflection;

using NLog.Targets;

namespace NLogFastCore.Builders
{
    /// <summary>
    /// Конструктор параметров
    /// </summary>
    public sealed class ParameterInfoBuilder
    {
        /// <summary>
        /// Перечисление параметров для таргета
        /// </summary>
        private readonly List<DatabaseParameterInfo> _parameterInfos = new();

        /// <summary>
        /// Последний параметр
        /// </summary>
        private DatabaseParameterInfo CurrentParameterInfo => _parameterInfos.Last();

        /// <summary>
        /// Установка Возможен ли null
        /// </summary>
        /// <param name="allowDbNull">Возможен ли null</param>
        /// <returns></returns>
        public ParameterInfoBuilder AllowDbNull(bool allowDbNull = true)
        {
            CurrentParameterInfo.AllowDbNull = allowDbNull;
            return this;
        }

        /// <summary>
        /// Установка Типа
        /// </summary>
        /// <param name="dbType">Тип</param>
        /// <returns></returns>
        internal ParameterInfoBuilder DbType(string dbType)
        {
            CurrentParameterInfo.DbType = dbType;
            return this;
        }

        /// <summary>
        /// Установка размера
        /// </summary>
        /// <param name="size">Размер</param>
        /// <returns></returns>
        public ParameterInfoBuilder Size(int size)
        {
            CurrentParameterInfo.Size = size;
            return this;
        }

        /// <summary>
        /// Установка названия
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns></returns>
        public ParameterInfoBuilder Name(string name)
        {
            CurrentParameterInfo.Name = name;
            return this;
        }

        /// <summary>
        /// Установка маркера
        /// </summary>
        /// <param name="layout">Маркер</param>
        /// <returns></returns>
        public ParameterInfoBuilder Layout(string layout)
        {
            CurrentParameterInfo.Name = layout;
            return this;
        }

        /// <summary>
        /// Получение последнего параметра
        /// </summary>
        /// <returns></returns>
        public DatabaseParameterInfo GetInstance()
            => GetCopy(CurrentParameterInfo);

        /// <summary>
        /// Добавление параметра
        /// </summary>
        /// <param name="parameterName">Название</param>
        /// <param name="parameterLayout">Маркер</param>
        /// <returns></returns>
        public ParameterInfoBuilder AddParameter(string parameterName, string parameterLayout)
        {
            _parameterInfos.Add(new DatabaseParameterInfo(parameterName, parameterLayout));
            return this;
        }

        /// <summary>
        /// Получение всех параметров
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DatabaseParameterInfo> GetInstances()
            => _parameterInfos.Select(GetCopy);

        /// <summary>
        /// Словарь типов
        /// </summary>
        private static readonly IDictionary<Type, IEnumerable<PropertyInfo>> TypeDictionary = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// Копирование
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private static T GetCopy<T>(T obj)
            where T : class, new()
        {
            var type = typeof(T);
            if (!TypeDictionary.TryGetValue(type, out var propertyInfos))
            {
                TypeDictionary.Add(type, type.GetProperties());
                propertyInfos = TypeDictionary[type];
            }
            if (obj == null)
            {
                throw new NullReferenceException("databaseParameterInfo cannot be null");
            }
            var newInstance = new T();
            foreach (var propertyInfo in propertyInfos)
            {
                propertyInfo.SetValue(newInstance, propertyInfo.GetValue(obj));
            }
            return newInstance;
        }
    }
}
