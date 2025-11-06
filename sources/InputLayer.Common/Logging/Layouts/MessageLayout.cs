using System.Text;

namespace InputLayer.Common.Logging.Layouts
{
    [LogLayout("message")]
    internal sealed class MessageLayout : ILogLayout
    {
        public void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat)
        {
            if (!string.IsNullOrWhiteSpace(logEvent.Message))
            {
                builder.Append(logEvent.Parameters?.Length > 0
                                   ? string.Format(logEvent.Message, logEvent.Parameters)
                                   : logEvent.Message);
            }

            if (logEvent.Exception == null)
            {
                return;
            }

            builder.AppendLine(logEvent.Exception.Message);
            builder.AppendLine(logEvent.Exception.StackTrace);
        }
    }
}