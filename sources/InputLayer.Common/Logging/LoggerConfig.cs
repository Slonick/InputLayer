using System.Collections.Generic;
using System.Reflection;
using InputLayer.Common.Logging.Writers;

namespace InputLayer.Common.Logging
{
    public class LoggerConfig
    {
        public LoggerConfig() { }

        public LoggerConfig(bool includeLogOriginDetails, LogLevel minLogLevel)
        {
            this.IncludeLogOriginDetails = includeLogOriginDetails;
            this.MinLogLevel = minLogLevel;
        }

        public Assembly Assembly { get; set; }

        public bool IncludeLogOriginDetails { get; set; }

        public string Layout { get; set; }

        public IList<ILogWriter> LogWriters { get; set; }

        public LogLevel MinLogLevel { get; set; }
    }
}