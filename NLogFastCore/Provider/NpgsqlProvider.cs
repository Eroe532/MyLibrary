using System.Text;

namespace NLogFastCore.Provider
{
    internal class NpgsqlProvider : IDbProviderSpecificFunctionality
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string StringDbType => "NpgsqlDbType.Text";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string DateTimeDbType => "NpgsqlDbType.Timestamp";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GuidDbType => "NpgsqlDbType.Uuid";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string IntDbType => "NpgsqlDbType.Integer";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string LongDbType => "NpgsqlDbType.Bigint";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string StringToken => "Npgsql.NpgsqlConnection, Npgsql";

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
            result.Append(GetQuoted(tableName));
            result.Append(space);
            if (columns != null && columns.Length > 0)
            {
                result.Append('(');
                result.Append(string.Join(',', columns.Select(GetQuoted)));
                result.Append(") ");
            }
            result.Append("VALUES ");
            result.Append('(');
            result.Append(string.Join(',', values));
            result.Append(");");
            return result.ToString();
        }

        /// <summary>
        /// Обертка слова в ковычки
        /// </summary>
        /// <param name="input">слово</param>
        /// <returns></returns>
        private static string GetQuoted(string input)
            => $"\"{input}\"";
    }
}
