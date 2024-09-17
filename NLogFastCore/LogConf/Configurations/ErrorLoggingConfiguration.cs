using NLog.Config;
using NLog.Targets;

using NLogFastCore.Builders.Targets;

namespace NLogFastCore.LogConf.Configurations
{
    /// <summary>
    /// Логгер ошибок
    /// </summary>
    public class ErrorLoggingConfiguration : BaseLoggingConfiguration
    {
        /// <summary>
        /// Логгер ошибок
        /// </summary>
        /// <param name="targetBuilder">Target-Конструктор </param>
        public ErrorLoggingConfiguration(ITargetBuilder targetBuilder) : base(targetBuilder)
        {
        }

        /// <inheritdoc/>
        protected override string TopicForRabbitMQ => "ErrorLogEvent";


        ///<inheritdoc/>
        protected override IEnumerable<Target> GetTargets()
        {
            TargetBuilder.AddSimpleParameter("LogDate", "${date}");
            TargetBuilder.AddSimpleParameter("Level", "${level:format=Name}");
            TargetBuilder.AddSimpleParameter("AppDomain", "${appdomain}");
            TargetBuilder.AddSimpleParameter("HostName", "${hostname}");
            TargetBuilder.AddSimpleParameter("LocalIp", "${local-ip}");
            TargetBuilder.AddSimpleParameter("MachineName", "${machinename}");
            TargetBuilder.AddSimpleParameter("AspnetEnvironment", "${aspnet-environment}", TargetBuilder.DBProviderSpecificFunctionality.StringDbType, true);
            TargetBuilder.AddSimpleParameter("CurrentDir", "${currentdir}");
            TargetBuilder.AddSimpleParameter("Logger", "${logger}");
            TargetBuilder.AddSimpleParameter("MethodParameters", "${Request-Parameters}", TargetBuilder.DBProviderSpecificFunctionality.StringDbType, true);
            TargetBuilder.AddSimpleParameter("RequestParameters", "${aspnet-request-routeparameters:OutputFormat=JsonArray}", TargetBuilder.DBProviderSpecificFunctionality.StringDbType, true);
            TargetBuilder.AddSimpleParameter("SessionID", "${aspnet-sessionid}", TargetBuilder.DBProviderSpecificFunctionality.StringDbType, true, 16);
            TargetBuilder.AddSimpleParameter("CallSite", "${callsite:filename=true}");
            TargetBuilder.AddSimpleParameter("Message", "${message}");
            TargetBuilder.AddSimpleParameter("Exception", "${exception:format=tostring:maxInnerExceptionLevel=16}");
            TargetBuilder.AddSimpleParameter("Stacktrace", "${stacktrace}");
            TargetBuilder.AddSimpleParameter("EventProperties", "${all-event-properties}");
            TargetBuilder.AddSimpleParameter("ObjectType", "${Exception-Object-Type}", TargetBuilder.DBProviderSpecificFunctionality.StringDbType, true);
            TargetBuilder.AddSimpleParameter("MethodName", "${Exception-Method}", TargetBuilder.DBProviderSpecificFunctionality.StringDbType, true);
            return new[] { TargetBuilder.GetTarget() };
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
