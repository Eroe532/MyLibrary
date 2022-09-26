using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using NLog;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;

namespace NLogFastCore.Layouts
{
    /// <summary>
    /// Название типа объекта
    /// </summary>
    [LayoutRenderer("Request-parameters")]
    internal class RequestParametersLayoutRenderer : AspNetLayoutRendererBase
    {
        /// <summary>
        /// Короткое название типа
        /// </summary>
        public bool Short { get; set; }

        ///<inheritdoc/>
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var parameters = new Dictionary<string, string>();
            var httpContext = HttpContextAccessor!.HttpContext;

            if (httpContext == null)
            {
                return;
            }

            if (httpContext.Items != null && httpContext.Items.Count > 0)
            {
                var found = httpContext.Items.SingleOrDefault(x => x.Value is IUrlHelper).Value;
                if (found is IUrlHelper urlHelper)
                {
                    builder.Append(Short
                        ? string.Join("; ", urlHelper.ActionContext.ModelState.Select(x => $"[\"{x.Value?.RawValue}\"]"))
                        : string.Join("; ", urlHelper.ActionContext.ModelState.Select(x => $"[\"{x.Key}\" = \"{x.Value?.RawValue}\"]")));
                    return;
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
                    parameters.Add(key, string.Join(", ", value));
                }
            }

            builder.Append(parameters.Count > 0 
                ? Short
                    ? string.Join("; ", parameters.Select(x => $"[\"{x.Value}\"]"))
                    : string.Join("; ", parameters.Select(x => $"[\"{x.Key}\"=\"{x.Value}\"]"))
                : null);
        }
    }
}
