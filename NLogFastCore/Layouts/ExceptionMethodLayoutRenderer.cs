using System.Text;

using NLog;
using NLog.LayoutRenderers;

namespace NLogFastCore.Layouts
{
    /// <summary>
    /// Название метода или сигнатура
    /// </summary>
    [LayoutRenderer("Exception-Method")]
    internal class ExceptionMethodLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Отображать сигнатуру метода
        /// </summary>
        public bool Signature { get; set; }

        ///<inheritdoc/>
        protected override void Append(StringBuilder builder, LogEventInfo logEventInfo)
        {
            if (logEventInfo?.Exception?.TargetSite is { } targetSite)
            {
                builder.Append(Signature ? targetSite.ToString() : targetSite.Name);
            }
        }
    }
}
