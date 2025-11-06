using System.Text;

namespace InputLayer.Common.Logging.Layouts
{
    public interface ILogLayout
    {
        void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat);
    }
}