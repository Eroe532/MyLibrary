using System.Data;

using NLog.Targets;

using NLogFastCore.DBProvider;

namespace NLogFastCore.Builders.Targets
{
    /// <summary>
    /// Класс конструктор DatabaseTarget
    /// </summary>
    public class DatabaseTargetBuilder : TargetBuilder
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        readonly string _connectionString;

        /// <summary>
        /// Название Таргета
        /// </summary>
        readonly string _targetName;

        /// <summary>
        /// Провайдер БД
        /// </summary>
        readonly IDbProviderSpecificFunctionality _dbProvider;

        /// <inheritdoc/>
        public override IDbProviderSpecificFunctionality DBProviderSpecificFunctionality => _dbProvider;

        /// <summary>
        /// Название таблицы
        /// </summary>
        readonly string _tableName;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetName">Название Таргета</param>
        /// <param name="dbProvider">Провайдер БД</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="tableName">Название таблицы</param>
        public DatabaseTargetBuilder(string targetName, IDbProviderSpecificFunctionality dbProvider, string connectionString, string tableName) : base()
        {
            _targetName = targetName;
            _dbProvider = dbProvider;
            _connectionString = connectionString;
            _tableName = tableName;
        }

        /// <inheritdoc/>
        public override Target GetTarget()
        {
            var target = new DatabaseTarget(_targetName)
            {
                ConnectionString = _connectionString,
                DBProvider = _dbProvider.StringToken,
                CommandType = CommandType.Text,
                CommandText = _dbProvider.GetInsertCommand(_tableName, _commandValues.ToArray(), _commandColumns.ToArray())
            };
            foreach (var parameterInfo in _targetParameters.GetInstances())
            {
                target.Parameters.Add(parameterInfo.ToDatabaseParameter());
            }
            return target;
        }
    }
}
