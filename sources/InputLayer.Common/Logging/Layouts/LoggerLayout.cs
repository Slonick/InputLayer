using System.Text;

namespace InputLayer.Common.Logging.Layouts
{
    [LogLayout("logger")]
    internal sealed class LoggerLayout : ILogLayout
    {
        public void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat)
        {
            if (layoutFormat == "short")
            {
                var index = this.TryGetLastDotForShortName(logEvent);
                if (index >= 0)
                {
                    builder.Append(logEvent.LoggerName, index + 1, logEvent.LoggerName.Length - index - 1);
                    return;
                }
            }

            builder.Append(logEvent.LoggerName);
        }

        private int TryGetLastDotForShortName(LogEventInfo logEvent)
        {
            var loggerName = logEvent.LoggerName;
            if (loggerName == null)
            {
                return -1;
            }

            return loggerName.LastIndexOf('.');
        }
    }
}