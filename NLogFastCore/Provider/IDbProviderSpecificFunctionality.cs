namespace NLogFastCore.Provider
{
    /// <summary>
    /// Провайдер базы данных
    /// </summary>
    public interface IDbProviderSpecificFunctionality
    {
        /// <summary>
        /// Тип String
        /// </summary>
        public string StringDbType { get; }

        /// <summary>
        /// Тип DateTime
        /// </summary>
        public string DateTimeDbType { get; }

        /// <summary>
        /// Тип Guid
        /// </summary>
        public string GuidDbType { get; }

        /// <summary>
        /// Тип Int
        /// </summary>
        public string IntDbType { get; }

        /// <summary>
        /// Тип Long
        /// </summary>
        public string LongDbType { get; }

        /// <summary>
        /// Название провайдера
        /// </summary>
        public string StringToken { get; }

        /// <summary>
        /// Получить запрос к бд
        /// </summary>
        /// <param name="tableName">Название таблици</param>
        /// <param name="values">Значения</param>
        /// <param name="columns">Столбцы</param>
        /// <returns></returns>
        public string GetInsertCommand(string tableName, string[] values, string[]? columns = null);
    }
}