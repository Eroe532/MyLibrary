using Nlog.RabbitMQ.Target;

using NLog.Targets;

namespace NLogFastCore.Builders.Parameters
{
    /// <summary>
    /// Параметр
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Layout
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// Тип для явной типизации
        /// </summary>
        public string? DbType { get; set; }

        /// <summary>
        /// Может ли быть пустым для явной типизации
        /// </summary>
        public bool? AllowDbNull { get; set; }

        /// <summary>
        /// Размер для явной типизации
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Параметр
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="layout">Layout</param>
        /// <param name="dbType">Тип для явной типизации</param>
        /// <param name="allowDbNull">Может ли быть пустым для явной типизации</param>
        /// <param name="size">Размер для явной типизации</param>
        public ParameterInfo(string name, string layout, string? dbType = null, bool? allowDbNull = null, int? size = null)
        {
            Name = name;
            Layout = layout;
            DbType = dbType;
            AllowDbNull = allowDbNull;
            Size = size;
        }

        /// <summary>
        /// Параметр
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="layout">Layout</param>
        public ParameterInfo(string name, string layout)
        {
            Name = name;
            Layout = layout;
        }

        /// <summary>
        /// Параметр
        /// </summary>
        public ParameterInfo()
        {
            Name = "";
            Layout = "";
        }

        /// <summary>
        /// Получить параметр для базы данных
        /// </summary>
        /// <returns></returns>
        public DatabaseParameterInfo ToDatabaseParameter()
        {
            var res = new DatabaseParameterInfo(Name, Layout);
            if (DbType != null)
            {
                res.DbType = DbType!;
            }
            if (AllowDbNull != null)
            {
                res.AllowDbNull = AllowDbNull.Value;
            }
            if (Size != null)
            {
                res.Size = Size.Value;
            }
            return res;
        }

        /// <summary>
        /// Получить параметр для RabbitMQ
        /// </summary>
        /// <returns></returns>
        public Field ToRabbitMQParameter()
        {
            return new Field(Name, Name, Layout);
        }

    }
}
