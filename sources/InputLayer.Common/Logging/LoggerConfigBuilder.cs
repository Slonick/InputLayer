using System.Collections.Generic;
using System.Reflection;
using InputLayer.Common.Logging.Writers;

namespace InputLayer.Common.Logging
{
    public static class LoggerConfigBuilder
    {
        public static LoggerConfig AddAssembly(this LoggerConfig loggerConfig, Assembly assembly)
        {
            loggerConfig.Assembly = assembly;

            return loggerConfig;
        }

        public static LoggerConfig Create()
            => new LoggerConfig();

        public static LoggerConfig Create(bool includeLogOriginDetails, LogLevel minLogLevel)
            => new LoggerConfig(includeLogOriginDetails, minLogLevel);

        public static LoggerConfig IncludeLogOriginDetails(this LoggerConfig loggerConfig, bool includeLogOriginDetails)
        {
            loggerConfig.IncludeLogOriginDetails = includeLogOriginDetails;
            return loggerConfig;
        }

        public static LoggerConfig MinLogLevel(this LoggerConfig loggerConfig, LogLevel minLogLevel)
        {
            loggerConfig.MinLogLevel = minLogLevel;
            return loggerConfig;
        }

        public static LoggerConfig UseConsoleWriter(this LoggerConfig loggerConfig)
        {
            if (loggerConfig.LogWriters == null)
            {
                loggerConfig.LogWriters = new List<ILogWriter>();
            }

            loggerConfig.LogWriters.Add(new ConsoleWriter());
            return loggerConfig;
        }

        public static LoggerConfig UseDebugLogger(this LoggerConfig loggerConfig)
        {
            if (loggerConfig.LogWriters == null)
            {
                loggerConfig.LogWriters = new List<ILogWriter>();
            }

            loggerConfig.LogWriters.Add(new DebugWriter());
            return loggerConfig;
        }

        public static LoggerConfig UseFileLogger(this LoggerConfig loggerConfig, string filepath, long maxFileSize = 10 * 1024 * 1024)
        {
            if (loggerConfig.LogWriters == null)
            {
                loggerConfig.LogWriters = new List<ILogWriter>();
            }

            loggerConfig.LogWriters.Add(new FileWriter(filepath, maxFileSize));
            return loggerConfig;
        }

        public static LoggerConfig UsePlayniteLogger(this LoggerConfig loggerConfig, Playnite.SDK.ILogger logger)
        {
            if (loggerConfig.LogWriters == null)
            {
                loggerConfig.LogWriters = new List<ILogWriter>();
            }

            loggerConfig.LogWriters.Add(new PlayniteWriter(logger));
            return loggerConfig;
        }

        public static LoggerConfig WithLayout(this LoggerConfig loggerConfig, string layout = "[${assembly-name,-16} ${assembly-version}] [${longdate,24}] [${level,-5}] ${logger:short}: ${message}")
        {
            loggerConfig.Layout = layout;
            return loggerConfig;
        }
    }
}