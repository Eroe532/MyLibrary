
using System.Data;

using NLog.Targets;

using NLogFastCore.Provider;

namespace NLogFastCore.Builders
{
    /// <summary>
    /// Класс конструктор DatabaseTarget
    /// </summary>
    public class DatabaseTargetBuilder
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

        /// <summary>
        /// Название таблицы
        /// </summary>
        readonly string _tableName;

        /// <summary>
        /// Перечисление значений столбцов в таблице
        /// </summary>
        readonly List<string> _commandValues;

        /// <summary>
        /// Перечисление столбцов в таблице
        /// </summary>
        readonly List<string> _commandColumns;

        /// <summary>
        /// Конструктор параметров
        /// </summary>
        readonly ParameterInfoBuilder _targetParameters;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetName">Название Таргета</param>
        /// <param name="dbProvider">Провайдер БД</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="tableName">Название таблицы</param>
        public DatabaseTargetBuilder(string targetName, IDbProviderSpecificFunctionality dbProvider, string connectionString, string tableName)
        {
            _targetName = targetName;
            _dbProvider = dbProvider;
            _connectionString = connectionString;
            _tableName = tableName;
            _targetParameters = new ParameterInfoBuilder();
            _commandValues = new List<string>();
            _commandColumns = new List<string>();
        }

        /// <summary>
        /// Добавление простого параметра
        /// </summary>
        /// <param name="parameterName">Название параметра</param>
        /// <param name="parameterLayout">Маркер параметра</param>
        /// <param name="dbType">Тип параметра</param>
        /// <param name="allowDbNull">Возможен ли null</param>
        /// <param name="size">Размер</param>
        public void AddSimpleParameter(string parameterName, string parameterLayout, string? dbType = null, bool? allowDbNull = null, int? size = null)
        {
            var parameterValue = "@" + parameterName;
            _targetParameters.AddParameter(parameterValue, parameterLayout);
            if (dbType != null)
            {
                _targetParameters.DbType(dbType);
            };
            if (allowDbNull != null)
            {
                _targetParameters.AllowDbNull(allowDbNull.Value);
            };
            if (size != null)
            {
                _targetParameters.Size(size.Value);
            }
            _commandValues.Add(parameterValue);
            _commandColumns.Add(parameterName);
        }

        /// <summary>
        /// Генерация Таргета
        /// </summary>
        /// <returns></returns>
        public Target GetTarget()
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
                target.Parameters.Add(parameterInfo);
            }
            return target;
        }
    }
}
