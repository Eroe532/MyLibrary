namespace DictionaryManagment.Model
{
    /// <summary>
    /// Интерфей для модели с полем ключа
    /// </summary>
    /// <typeparam name="U">Тип ключа</typeparam>
    public interface IDictionaryModel<U> where U : struct
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public U Id { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string Title { get; set; }
    }
}


