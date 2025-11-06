using System;
using System.Reflection;

namespace InputLayer.Common.Logging
{
    public class LogEventInfo
    {
        public LogEventInfo(LogLevel level, string message, object[] parameters, Exception exception = null)
        {
            this.Level = level;
            this.Message = message;
            this.Parameters = parameters;
            this.Exception = exception;
        }

        public LogEventInfo() { }
        public Exception Exception { get; }
        public LogLevel Level { get; }
        public string Message { get; }
        public object[] Parameters { get; }
        public Assembly Assembly { get; set; }
        public string CallerFilePath { get; set; }
        public int CallerLineNumber { get; set; }
        public string CallerMemberName { get; set; }
        public string LoggerName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}