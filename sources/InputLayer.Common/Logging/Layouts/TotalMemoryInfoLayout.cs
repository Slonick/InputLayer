using System;
using System.Text;

namespace InputLayer.Common.Logging.Layouts
{
    [LogLayout("memory")]
    internal sealed class TotalMemoryInfoLayout : ILogLayout
    {
        public void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat)
        {
            var bytes = GC.GetTotalMemory(false);
            builder.Append(FormatBytes(bytes));
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            var order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}