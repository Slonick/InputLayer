using System;

namespace InputLayer.Common.Logging.Writers
{
    public class ConsoleWriter : ILogWriter
    {
        public void Write(string line) => Console.WriteLine(line);
    }
}