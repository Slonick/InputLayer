using System.Collections.Generic;
using InputLayer.Common.Logging.Layouts;

namespace InputLayer.Common.Logging
{
    public interface ILogFactory
    {
        LoggerConfig Config { get; set; }

        ILogger GetLogger(string name);

        IDictionary<string, ILogLayout> GetLogLayouts();
    }
}