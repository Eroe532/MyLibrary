using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using NLog;
using NLog.Config;
using NLog.Web;

namespace NLogFastCore.Layouts
{
    /// <summary>
    /// Регистратор маркеров
    /// </summary>
    internal static class LayoutsRegistration
    {
        /// <summary>
        /// Выполнено или нет
        /// </summary>
        private static bool LayoutsRegistered;

        /// <summary>
        /// Регистрация
        /// </summary>
        public static ISetupBuilder RegisterLayouts(this ISetupBuilder setupBuilder)
        {
            if (LayoutsRegistered)
            {
                return setupBuilder;
            }
            setupBuilder.SetupExtensions(x => x.RegisterAspNetLayoutRenderer("Request-Parameters", (logEventInfo, httpContext, loggingConfiguration) =>
            {
                var parameters = new Dictionary<string, string>();

                if (httpContext == null)
                {
                    return null;
                }

                if (httpContext.Items != null && httpContext.Items.Count > 0)
                {
                    var found = httpContext.Items.SingleOrDefault(x => x.Value is IUrlHelper).Value;
                    if (found is IUrlHelper urlHelper)
                    {
                        return string.Join("; ", urlHelper.ActionContext.ModelState.Select(x => $"[\"{x.Key}\" = \"{x.Value?.RawValue}\"]"));
                    }
                }

                var routeData = httpContext.GetRouteData();
                if (routeData?.Values != null && routeData.Values.Count > 0)
                {
                    var keys = routeData.Values.Keys.ToArray();
                    var values = routeData.Values.Values.ToArray();
                    for (var i = 0; i < keys.Length; i++)
                    {
                        var routeKey = keys[i];
                        if (routeKey.Contains("controller", StringComparison.InvariantCultureIgnoreCase) ||
                            routeKey.Contains("action", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        parameters.Add(routeKey, values[i]?.ToString() ?? "");
                    }
                }

                if (httpContext.Request?.Query != null && httpContext.Request.Query.Count > 0)
                {
                    foreach (var (key, value) in httpContext.Request.Query)
                    {
                        parameters.Add(key, string.Join(", ", value.Where(v => v is not null).ToArray()));
                    }
                }

                return parameters.Count > 0 ? $"{string.Join("; ", parameters.Select(x => $"[\"{x.Key}\"=\"{x.Value}\"]"))} " : null;
            }));

            setupBuilder.SetupExtensions(x => x.RegisterLayoutRenderer<ExceptionMethodLayoutRenderer>("Exception-Method"));
            setupBuilder.SetupExtensions(x => x.RegisterLayoutRenderer<ExceptionObjectTypeLayoutRenderer>("Exception-Object-Type"));
            LayoutsRegistered = true;
            return setupBuilder;
        }
    }
}
