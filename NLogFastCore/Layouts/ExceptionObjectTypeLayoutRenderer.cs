using System.Text;

using NLog;
using NLog.LayoutRenderers;

namespace NLogFastCore.Layouts
{
    /// <summary>
    /// Название типа объекта
    /// </summary>
    [LayoutRenderer("Exception-Object-Type")]
    internal class ExceptionObjectTypeLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Короткое название типа
        /// </summary>
        public bool Short { get; set; }

        ///<inheritdoc/>
        protected override void Append(StringBuilder builder, LogEventInfo logEventInfo)
        {
            if (logEventInfo?.Exception?.TargetSite?.DeclaringType is { } declaringType)
            {
                builder.Append(Short ? declaringType.Name : declaringType.FullName);
            }
        }
    }
}
