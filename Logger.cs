

namespace Armory
{
    /// <summary>
    /// Facilitates Logging
    /// </summary>
    public class Logger
    {
        private static log4net.ILog DBLog = Zeta.Common.Logger.GetLoggerInstanceForType();

        private static string _lastDebugLog = "";
        private static string _lastInfoLog = "";
        private static string _lastErrorLog = "";

        private static string _logName;
        private static string LogName
        {
            get
            {
                if (!string.IsNullOrEmpty(_logName))
                    return _logName;

                _logName = "[" + Plugin.NAME + "] ";
                return _logName;
            }
        }

        /// <summary>
        /// Write to Error (Quiet)
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Error(message, null);
        }

        /// <summary>
        /// Write to Error (Quiet)
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message, params object[] args)
        {
            if (args != null)
                message = string.Format(LogName + message, args);
            if (message != _lastErrorLog)
            {
                _lastErrorLog = message;
                DBLog.Error(message);
            }
        }

        /// <summary>
        /// Write to Normal
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            Log(message, null);
        }

        /// <summary>
        /// Write to Normal
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message, params object[] args)
        {
            if (args != null)
                message = string.Format(LogName + message, args);
            if (message != _lastInfoLog)
            {
                _lastInfoLog = message;
                DBLog.Info(message);
            }
        }

        /// <summary>
        /// Write to Verbose
        /// </summary>
        /// <param name="message"></param>
        public static void Verbose(string message)
        {
            Verbose(message, null);
        }

        /// <summary>
        /// Write to Verbose
        /// </summary>
        /// <param name="message"></param>
        public static void Verbose(string message, params object[] args)
        {
            if (args != null)
                message = string.Format(LogName + message, args);
            if (message != _lastDebugLog)
            {
                _lastDebugLog = message;
                DBLog.Debug(message);
            }
        }

        /// <summary>
        /// Write to Debug (Diagnostic)
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            Verbose(message, null);
        }

        /// <summary>
        /// Write to Debug (Diagnostic)
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message, params object[] args)
        {
            if (args != null)
                message = string.Format(LogName + message, args);
            if (message != _lastDebugLog)
            {
                _lastDebugLog = message;
                DBLog.Debug(message);
            }
        }
    }
}
