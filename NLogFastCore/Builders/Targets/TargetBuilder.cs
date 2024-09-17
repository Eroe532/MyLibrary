using NLog.Targets;

using NLogFastCore.Builders.Parameters;
using NLogFastCore.DBProvider;

namespace NLogFastCore.Builders.Targets
{
    /// <summary>
    /// Класс конструктор DatabaseTarget
    /// </summary>
    public abstract class TargetBuilder : ITargetBuilder
    {
        /// <inheritdoc/>
        public abstract IDbProviderSpecificFunctionality DBProviderSpecificFunctionality { get; }

        /// <summary>
        /// Перечисление значений столбцов в таблице
        /// </summary>
        protected List<string> _commandValues;

        /// <summary>
        /// Перечисление столбцов в таблице
        /// </summary>
        protected List<string> _commandColumns;

        /// <summary>
        /// Конструктор параметров
        /// </summary>
        protected ParameterInfoBuilder _targetParameters;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TargetBuilder()
        {
            _targetParameters = new ParameterInfoBuilder();
            _commandValues = new List<string>();
            _commandColumns = new List<string>();
        }

        /// <inheritdoc/>
        public void AddSimpleParameter(string parameterName, string parameterLayout, string? dbType = null, bool? allowDbNull = null, int? size = null)//todo переименовать
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

        /// <inheritdoc/>
        public abstract Target GetTarget();
    }
}
