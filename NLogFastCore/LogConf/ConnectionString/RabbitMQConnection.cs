namespace NLogFastCore.LogConf.ConnectionString
{
    /// <summary>
    /// Строка подключения к RabbitMQ логгера
    /// </summary>
    public class RabbitMQConnection
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        public RabbitMQConnection()
        {
        }

        /// <summary>
        /// Десериализация из json
        /// </summary>
        /// <param name="connectionString">Строка</param>
        /// <returns></returns>
        public RabbitMQConnection(string connectionString)
        {
            try
            {
                var parameters = connectionString.Split(';').Select(p => p.Split('=')).ToDictionary(d => d[0].Trim().ToLower(), d => d[1].Trim());
                if (parameters.TryGetValue("hostname", out var hostName))
                {
                    HostName = hostName;
                }
                if (parameters.TryGetValue("username", out var userName))
                {
                    UserName = userName;
                }
                if (parameters.TryGetValue("password", out var password))
                {
                    Password = password;
                }
                if (parameters.TryGetValue("port", out var port))
                {
                    Port = port;
                }
                if (parameters.TryGetValue("vhost", out var vHost))
                {
                    VHost = vHost;
                }
                if (parameters.TryGetValue("exchange", out var exchange))
                {
                    Exchange = exchange;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка в строке подключения", ex);
            }
        }

        /// <summary>
        /// Имя хоста
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; } = "guest";

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = "guest";

        /// <summary>
        /// Порт
        /// </summary>
        public string Port { get; set; } = "5672";

        //todo переименовать
        /// <summary>
        /// vHost
        /// </summary>
        public string VHost { get; set; } = "/";

        //todo переименовать
        /// <summary>
        /// exchange
        /// </summary>
        public string Exchange { get; set; } = "Error";
    }
}
