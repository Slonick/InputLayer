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

        /// <summary>
        ///     If true, includes the origin of where the log message was logged from
        ///     such as the class name, line number and file name
        /// </summary>
        public bool IncludeLogOriginDetails { get; set; }

        /// <summary>
        ///     Log string layout
        /// </summary>
        public string Layout { get; set; }

        public IList<ILogWriter> LogWriters { get; set; }

        /// <summary>
        ///     The level of logging to output
        /// </summary>
        public LogLevel MinLogLevel { get; set; }
    }
}