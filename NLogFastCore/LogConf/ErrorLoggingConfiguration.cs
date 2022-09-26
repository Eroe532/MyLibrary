using NLog.Config;
using NLog.Targets;

using NLogFastCore.Builders;
using NLogFastCore.Provider;

namespace NLogFastCore.LogConf
{
    /// <summary>
    /// Логгер ошибок
    /// </summary>
    public class ErrorLoggingConfiguration : BaseLoggingConfiguration
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        readonly string _tableName;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        public ErrorLoggingConfiguration(string tableName)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ErrorLoggingConfiguration(): this("ErrorLoggers")
        {
        }


        ///<inheritdoc/>
        protected override IEnumerable<Target> GetTargets(string connectionString, IDbProviderSpecificFunctionality dbProviderSpecificFunctionality)
        {
            var targetBuilder = new DatabaseTargetBuilder(
                "DefaultErrorLogger",
                dbProviderSpecificFunctionality,
                connectionString,
                _tableName);
            targetBuilder.AddSimpleParameter("LogDate", "${date}");
            targetBuilder.AddSimpleParameter("Level", "${level:format=Name}");
            targetBuilder.AddSimpleParameter("AppDomain", "${appdomain}");
            targetBuilder.AddSimpleParameter("HostName", "${hostname}");
            targetBuilder.AddSimpleParameter("LocalIp", "${local-ip}");
            targetBuilder.AddSimpleParameter("MachineName", "${machinename}");
            targetBuilder.AddSimpleParameter("AspnetEnvironment", "${aspnet-environment}", dbProviderSpecificFunctionality.StringDbType, true);
            targetBuilder.AddSimpleParameter("CurrentDir", "${currentdir}");
            targetBuilder.AddSimpleParameter("Logger", "${logger}");
            targetBuilder.AddSimpleParameter("MethodParameters", "${Request-parameters}", dbProviderSpecificFunctionality.StringDbType, true);
            targetBuilder.AddSimpleParameter("RequestParameters", "${aspnet-request-routeparameters:OutputFormat=JsonArray}", dbProviderSpecificFunctionality.StringDbType, true);
            targetBuilder.AddSimpleParameter("SessionID", "${aspnet-sessionid}", dbProviderSpecificFunctionality.StringDbType, true, 16);
            targetBuilder.AddSimpleParameter("CallSite", "${callsite:filename=true}");
            targetBuilder.AddSimpleParameter("Message", "${message}");
            targetBuilder.AddSimpleParameter("Exception", "${exception:format=tostring:maxInnerExceptionLevel=16}");
            targetBuilder.AddSimpleParameter("Stacktrace", "${stacktrace}");
            targetBuilder.AddSimpleParameter("EventProperties", "${all-event-properties}");
            targetBuilder.AddSimpleParameter("ObjectType", "${Exception-object-type-name}", dbProviderSpecificFunctionality.StringDbType, true);
            targetBuilder.AddSimpleParameter("MethodName", "${Exception-method}", dbProviderSpecificFunctionality.StringDbType, true);
            return new[] { targetBuilder.GetTarget() };
        }

        ///<inheritdoc/>
        protected override void SetLoggingRules(LoggingConfiguration loggingConfiguration, IEnumerable<Target> targets)
        {
            foreach (var target in targets)
            {
                loggingConfiguration.AddRule(_minLogLevel, _maxLogLevel, target);
            }
        }
    }
}
