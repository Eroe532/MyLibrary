using System.Text;

namespace NLogFastCore.DBProvider
{
    internal class MicrosoftSQLServerProvider : IDbProviderSpecificFunctionality
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string StringDbType => "DbType.String";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string DateTimeDbType => "DbType.DateTime";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GuidDbType => "DbType.Guid";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string IntDbType => "DbType.Int32";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string LongDbType => "DbType.Int64";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string StringToken => "System.Data.SqlClient";

        /// <summary>
        /// Пробел
        /// </summary>
        private static readonly char space = ' ';

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GetInsertCommand(string tableName, string[] values, string[]? columns = null)
        {
            var result = new StringBuilder();


            result.Append("INSERT INTO ");
            result.Append(tableName);
            result.Append(space);
            if (columns != null && columns.Length > 0)
            {
                result.Append('(');
                result.Append(string.Join(',', columns));
                result.Append(") ");
            }
            result.Append("VALUES ");
            result.Append('(');
            result.Append(string.Join(',', values));
            result.Append(");");
            return result.ToString();
        }
    }
}