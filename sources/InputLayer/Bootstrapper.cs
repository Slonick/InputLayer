using System.Reflection;
using InputLayer.Common.Constants;
using InputLayer.Common.Logging;
using LogManager = Playnite.SDK.LogManager;

namespace InputLayer
{
    public static class Bootstrapper
    {
        public static void Setup()
        {
            var loggerConfig = LoggerConfigBuilder.Create(false, LogLevel.Info)
                                                  #if DEBUG
                                                  .IncludeLogOriginDetails(true)
                                                  .MinLogLevel(LogLevel.Trace)
                                                  #endif
                                                  .AddAssembly(Assembly.GetAssembly(typeof(Bootstrapper)))
                                                  .WithLayout()
                                                  .UseDebugLogger()
                                                  .UsePlayniteLogger(LogManager.GetLogger())
                                                  .UseFileLogger(PathConstants.LogFile);

            Common.Logging.LogManager.Default.SetConfig(loggerConfig);
        }
    }
}