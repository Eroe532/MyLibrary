using NLog;

namespace NLogFastCore
{
    /// <summary>
    /// InternalLogging
    /// </summary>
    public static class InternalLoggingConfiguration
    {
        private static bool InternalLoggingSetup;

        /// <summary>
        /// Установка
        /// </summary>
        public static void Setup()
        {
            if (InternalLoggingSetup)
            {
                return;
            }
            NLog.Common.InternalLogger.LogLevel = LogLevel.Info;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                NLog.Common.InternalLogger.LogFile = $@"C:\Users\Public\{AppDomain.CurrentDomain.FriendlyName}.log";
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                NLog.Common.InternalLogger.LogFile = $@"/tmp/{AppDomain.CurrentDomain.FriendlyName}.log";
            }
            InternalLoggingSetup = true;
        }
    }
}
