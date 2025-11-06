using System;
using System.Reflection;

namespace InputLayer.Common.Logging
{
    public static class LogEventInfoBuilder
    {
        public static LogEventInfo Create()
            => new LogEventInfo();

        public static LogEventInfo Create(LogLevel level, string message, object[] parameters, Exception exception = null)
            => new LogEventInfo(level, message, parameters, exception);

        public static LogEventInfo WithAssembly(this LogEventInfo logEventInfo, Assembly assembly)
        {
            logEventInfo.Assembly = assembly;
            return logEventInfo;
        }

        public static LogEventInfo WithCallerInfo(this LogEventInfo logEventInfo, string memberName, string filePath, int lineNumber)
        {
            logEventInfo.CallerMemberName = memberName;
            logEventInfo.CallerFilePath = filePath;
            logEventInfo.CallerLineNumber = lineNumber;
            return logEventInfo;
        }

        public static LogEventInfo WithCategory(this LogEventInfo logEventInfo, string category)
        {
            logEventInfo.LoggerName = category;
            return logEventInfo;
        }

        public static LogEventInfo WithTimeStamp(this LogEventInfo logEventInfo, DateTimeOffset dateTime)
        {
            logEventInfo.TimeStamp = dateTime;
            return logEventInfo;
        }
    }
}