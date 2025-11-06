using System.Reflection;
using InputLayer.Common.Constants;
using InputLayer.Common.Logging;

namespace InputLayer.Agent
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
                                                  .UseConsoleWriter()
                                                  .UseFileLogger(PathConstants.LogFile);

            LogManager.Default.SetConfig(loggerConfig);
        }
    }
}