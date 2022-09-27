namespace EventStoreDBLibrary.Exeptions
{
    /// <summary>
    /// Ошибка при получении события
    /// </summary>
    public class NotActualException : Exception
    {
        /// <summary>
        /// Ошибка при получении события
        /// </summary>
        public NotActualException() : base(ToMessage())
        {
        }


        /// <summary>
        /// Ошибка при получении события
        /// </summary>
        public NotActualException(Exception innerException) : base(ToMessage(), innerException)
        {
        }

        /// <summary>
        /// Генерирование сообщения
        /// </summary>
        /// <returns></returns>
        private static string ToMessage()
        {
            return $"Не удалось получить актуальные данные";
        }
    }
}