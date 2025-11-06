using System.ComponentModel;
using System.Text;
using InputLayer.Common.Extensions;

namespace InputLayer.Common.Logging.Layouts
{
    [LogLayout("longdate")]
    internal sealed class LongDateLogLayout : ILogLayout
    {
        [DefaultValue(false)]
        public bool UniversalTime { get; set; }

        public void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat)
        {
            var dateTime = logEvent.TimeStamp;
            if (this.UniversalTime)
            {
                dateTime = dateTime.ToUniversalTime();
            }

            builder.Append4DigitsZeroPadded(dateTime.Year);
            builder.Append('-');
            builder.Append2DigitsZeroPadded(dateTime.Month);
            builder.Append('-');
            builder.Append2DigitsZeroPadded(dateTime.Day);
            builder.Append(' ');
            builder.Append2DigitsZeroPadded(dateTime.Hour);
            builder.Append(':');
            builder.Append2DigitsZeroPadded(dateTime.Minute);
            builder.Append(':');
            builder.Append2DigitsZeroPadded(dateTime.Second);
            builder.Append('.');
            builder.Append4DigitsZeroPadded((int)(dateTime.Ticks % 10000000L) / 1000);
        }
    }
}