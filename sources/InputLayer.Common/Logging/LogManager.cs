using InputLayer.Common.Utils;

namespace InputLayer.Common.Logging
{
    public class LogManager
    {
        private static readonly object _syncObject = new object();
        private static LogManager _instance;
        private readonly ILogFactory _logFactory = LogFactory.Default;

        public LogManager(ILogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        private LogManager() { }

        public static LogManager Default
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public ILogger GetCurrentClassLogger()
            => _logFactory.GetLogger(StackTraceUtils.GetClassFullName());

        public void SetConfig(LoggerConfig loggerConfig)
            => _logFactory.Config = loggerConfig;
    }
}