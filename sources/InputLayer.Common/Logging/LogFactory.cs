using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InputLayer.Common.Extensions;
using InputLayer.Common.Logging.Layouts;

namespace InputLayer.Common.Logging
{
    public class LogFactory : ILogFactory
    {
        private static readonly object _syncObject = new object();
        private static ILogFactory _defaultInstance;
        private readonly ConcurrentDictionary<LoggerCacheKey, ILogger> _cache = new ConcurrentDictionary<LoggerCacheKey, ILogger>();
        private IDictionary<string, ILogLayout> _logLayoutsCache;

        public static ILogFactory Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (_syncObject)
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new LogFactory();
                        }
                    }
                }

                return _defaultInstance;
            }
        }

        public LoggerConfig Config { get; set; }

        public ILogger GetLogger(string name)
            => this.GetLoggerThreadSafe(name, Logger.DefaultLoggerType);

        public IDictionary<string, ILogLayout> GetLogLayouts()
        {
            if (_logLayoutsCache != null)
            {
                return _logLayoutsCache;
            }

            var safeAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                          .Where(a => !a.IsDynamic && a.FullName.StartsWith(nameof(InputLayer)));

            var layouts = new List<ILogLayout>();

            foreach (var assembly in safeAssemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                                        .Where(x => typeof(ILogLayout).IsAssignableFrom(x) &&
                                                    !x.IsInterface &&
                                                    !x.IsAbstract);

                    foreach (var type in types)
                    {
                        try
                        {
                            var instance = (ILogLayout)Activator.CreateInstance(type);
                            layouts.Add(instance);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var loadedTypes = ex.Types.Where(t => t != null &&
                                                          typeof(ILogLayout).IsAssignableFrom(t) &&
                                                          !t.IsInterface &&
                                                          !t.IsAbstract);
                    foreach (var type in loadedTypes)
                    {
                        try
                        {
                            var instance = (ILogLayout)Activator.CreateInstance(type);
                            layouts.Add(instance);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }

            _logLayoutsCache = layouts.ToDictionary(
                                                    x => x.GetType().GetAttributeOfType<LogLayoutAttribute>().Name,
                                                    x => x);

            return _logLayoutsCache;
        }

        private ILogger GetLoggerThreadSafe(string name, Type loggerType)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Name of logger cannot be null");
            }

            var loggerCacheKey = new LoggerCacheKey(name, loggerType ?? typeof(Logger));
            if (_cache.TryGetValue(loggerCacheKey, out var result))
            {
                return result;
            }

            loggerCacheKey = new LoggerCacheKey(loggerCacheKey.Name, typeof(Logger));
            var logger = new Logger();

            logger.Initialize(name, this);
            _cache.TryAdd(loggerCacheKey, logger);
            return logger;
        }
    }
}