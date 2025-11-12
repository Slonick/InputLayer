using System.Diagnostics;

namespace InputLayer.Common.Logging.Writers
{
    public class DebugWriter : ILogWriter
    {
        public void Write(string line)
            => Debug.WriteLine(line);
    }
}