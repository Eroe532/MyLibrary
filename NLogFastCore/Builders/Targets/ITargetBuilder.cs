using NLog.Targets;

using NLogFastCore.DBProvider;

namespace NLogFastCore.Builders.Targets
{
    /// <summary>
    /// Target-Конструктор 
    /// </summary>
    public interface ITargetBuilder
    {
        /// <summary>
        /// Класс для формарования кода запроса
        /// </summary>
        public IDbProviderSpecificFunctionality DBProviderSpecificFunctionality { get; }

        /// <summary>
        /// Добавление простого параметра
        /// </summary>
        /// <param name="parameterName">Название параметра</param>
        /// <param name="parameterLayout">Маркер параметра</param>
        /// <param name="dbType">Тип параметра</param>
        /// <param name="allowDbNull">Возможен ли null</param>
        /// <param name="size">Размер</param>
        public void AddSimpleParameter(string parameterName, string parameterLayout, string? dbType = null, bool? allowDbNull = null, int? size = null);

        /// <summary>
        /// Генерация Таргета
        /// </summary>
        /// <returns></returns>
        public Target GetTarget();
    }
}
