using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryManagment.Model;

namespace DictionaryManagment.Dictionary
{
    /// <summary>
    /// Словарь
    /// </summary>
    /// <typeparam name="Key">Ключ</typeparam>
    /// <typeparam name="Value">Значение</typeparam>
    public class SafeDictionary<Key, Value> : ConcurrentDictionary<Key, Value>, ISafeDictionary<Key, Value> where Value : class, IDictionaryModel<Key> where Key : struct
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dictionary"> Словарь </param>
        public SafeDictionary(IDictionary<Key, Value> dictionary) : base(dictionary) { }

        /// <summary>
        /// Взаимодействие по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public Value? Get(Key key)
        {
            return TryGetValue(key, out var value) ? value : null;
        }

        public void Set(Key key, Value value)
        {
            AddOrUpdate(key, factory => value, (keyValue, oldValue) => value);
        }


        /// <summary>
        /// неявное преобразование
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator SafeDictionary<Key, IDictionaryModel<Key>>(SafeDictionary<Key, Value> input)
        {
            return input;
        }
    }
}

