/*using System.Collections.Generic;
using System.Data;
using System.Linq;

using CaTLogging.DBProvider;

using NLog;
using NLog.Config;
using NLog.Targets;

namespace CaTLogging.LogConf
{
    public class MethodLoggingConfiguration : BaseLoggingConfiguration
    {
        protected override IEnumerable<Target> GetTargets(string connectionString, IDbProviderSpecificFunctionality dbProviderSpecificFunctionality)
        {
            var methodLoggerTarget = new DatabaseTarget("DefaultCaTMethodLogger")
            {
                ConnectionString = connectionString,
                DBProvider = dbProviderSpecificFunctionality.StringToken,
                CommandType = CommandType.Text,
                CommandText = dbProviderSpecificFunctionality.GetInsertCommand("MethodLoggers",
                    new[] { "@Controller + '/' + @Action)", "@MethodParameters", "@ClaimValueClientID", "@Date", "@ClaimValueWorkerID", "@SessionID" },
                    new[] { "MethodName", "MethodParams", "ClientId", "CallDate", "WorkerChangedBy", "SessionId" })
            };

            var methodLoggerTargetParameters = new ParameterInfoBuilder(ParametersFactory.GetNullableStringParameter(dbProviderSpecificFunctionality))
                .AddBasedParameter("@Controller", "${aspnet-mvc-controller}")
                .AddBasedParameter("@Action", "${aspnet-mvc-action}")
                .GetInstances().ToList();

            foreach(var parameterInfo in ParametersFactory.GetCommonParameters(dbProviderSpecificFunctionality))
                methodLoggerTarget.Parameters.Add(parameterInfo);

            foreach(var parameterInfo in methodLoggerTargetParameters)
                methodLoggerTarget.Parameters.Add(parameterInfo);

            return new[] { methodLoggerTarget };
        }

        protected override void SetLoggingRules(LoggingConfiguration loggingConfiguration, IEnumerable<Target> targets)
        {
            var loggerNamePattern = @"*.ControllerActionInvoker";
            foreach(var target in targets)
                loggingConfiguration.AddRuleForOneLevel(LogLevel.Info, target, loggerNamePattern);
        }
    }
}
*/