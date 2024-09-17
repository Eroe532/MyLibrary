using Nlog.RabbitMQ.Target;

using NLog.Targets;

using NLogFastCore.DBProvider;
using NLogFastCore.LogConf.ConnectionString;

namespace NLogFastCore.Builders.Targets
{
    /// <summary>
    /// Класс конструктор DatabaseTarget
    /// </summary>
    public class RabbitMQTargetBuilder : TargetBuilder
    {
        /// <inheritdoc/>
        public override IDbProviderSpecificFunctionality DBProviderSpecificFunctionality => NotDBProvider.Get();

        /// <summary>
        /// Название Таргета
        /// </summary>
        readonly string _targetName;

        /// <summary>
        /// Имя хоста
        /// </summary>
        readonly string _hostName;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        readonly string _userName;

        /// <summary>
        /// Пароль
        /// </summary>
        readonly string _password;

        /// <summary>
        /// Порт
        /// </summary>
        readonly string _port;

        //todo переименовать
        /// <summary>
        /// vHost
        /// </summary>
        readonly string _vHost;

        //todo переименовать
        /// <summary>
        /// exchange
        /// </summary>
        readonly string _exchange;

        internal string Topic { get; set; }

        //todo переименовать
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetName">Название Таргета</param>
        /// <param name="hostName">Имя хоста</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="port">Порт</param>
        /// <param name="vHost">vHost</param>
        /// <param name="exchange">exchange</param>
        public RabbitMQTargetBuilder(string targetName, string hostName, string userName, string password, string port, string vHost, string exchange) : base()
        {
            _targetName = targetName;
            _hostName = hostName;
            _userName = userName;
            _password = password;
            _port = port;
            _vHost = vHost;
            _exchange = exchange;
            Topic = "";
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetName">Название Таргета</param>
        /// <param name="rabbitMQConnection">Подключение</param>
        public RabbitMQTargetBuilder(string targetName, RabbitMQConnection rabbitMQConnection) : base()
        {
            _targetName = targetName;
            _hostName = rabbitMQConnection.HostName;
            _userName = rabbitMQConnection.UserName;
            _password = rabbitMQConnection.Password;
            _port = rabbitMQConnection.Port;
            _vHost = rabbitMQConnection.VHost;
            _exchange = rabbitMQConnection.Exchange;
            Topic = "";
        }

        /// <inheritdoc/>
        public override Target GetTarget()
        {
            var target = new RabbitMQTarget()
            {
                DeliveryMode = DeliveryMode.Persistent,
                HostName = _hostName,
                UserName = _userName,
                Password = _password,
                Port = _port,
                VHost = _vHost,
                Exchange = _exchange,
                Name = _targetName,
                UseJSON = true,
                Topic = Topic
            };
            foreach (var parameterInfo in _targetParameters.GetInstances())
            {
                target.Fields.Add(parameterInfo.ToRabbitMQParameter());
                target.ContextProperties.Add(new TargetPropertyWithContext(parameterInfo.Name, parameterInfo.Layout));
            }
            return target;
        }
    }
}
