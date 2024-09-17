namespace NLogFastCore.DBProvider
{
    /// <summary>
    /// Для тех случев когда не нужно составлять код вручную
    /// </summary>
    public class NotDBProvider : IDbProviderSpecificFunctionality
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string StringDbType => "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string DateTimeDbType => "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GuidDbType => "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string IntDbType => "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string LongDbType => "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string StringToken => "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GetInsertCommand(string tableName, string[] values, string[]? columns = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Статический объект
        /// </summary>
        private static NotDBProvider? notDBProvider;

        /// <summary>
        /// Получить
        /// </summary>
        /// <returns></returns>
        public static NotDBProvider Get()
        {
            return notDBProvider ??= new NotDBProvider();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        private NotDBProvider()
        {

        }
    }
}