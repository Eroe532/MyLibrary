
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;


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
        public static void RegisterLayouts()
        {
            if (LayoutsRegistered)
            {
                return;
            }
            LayoutRenderer.Register<RequestParametersLayoutRenderer>("Request-parameters");
            LayoutRenderer.Register<ExceptionMethodLayoutRenderer>("Exception-method");
            LayoutRenderer.Register<ExceptionObjectTypeLayoutRenderer>("Exception-object-type");

            LayoutsRegistered = true;
        }
    }
}
