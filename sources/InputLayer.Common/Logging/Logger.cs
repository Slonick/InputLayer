using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace InputLayer.Common.Logging
{
    public class Logger : ILogger
    {
        internal static readonly Type DefaultLoggerType = typeof(Logger);

        private ILogFactory _logFactory;

        public string Name { get; private set; }

        public void Debug(Exception exception,
                          string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Debug, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Debug(string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Debug, message, argument)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Error(Exception exception,
                          string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Error, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Error(string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Error, message, argument)
                                          .WithCategory(this.Name).WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Fatal(Exception exception,
                          string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Fatal, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Fatal(string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Fatal, message, argument)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Info(Exception exception,
                         string message,
                         object[] argument,
                         [CallerMemberName] string memberName = "",
                         [CallerFilePath] string filePath = "",
                         [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Info, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Info(string message,
                         object[] argument,
                         [CallerMemberName] string memberName = "",
                         [CallerFilePath] string filePath = "",
                         [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Info, message, argument)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Initialize(string name, ILogFactory logFactoryInstance)
        {
            this.Name = name;
            _logFactory = logFactoryInstance;
        }

        public void Log(LogLevel logLevel,
                        Exception exception,
                        string message,
                        object[] argument,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string filePath = "",
                        [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(logLevel, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Log(LogLevel logLevel,
                        string message,
                        object[] argument,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string filePath = "",
                        [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(logLevel, message, argument)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Trace(Exception exception,
                          string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Trace, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Trace(string message,
                          object[] argument,
                          [CallerMemberName] string memberName = "",
                          [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Trace, message, argument)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Warn(Exception exception,
                         string message,
                         object[] argument,
                         [CallerMemberName] string memberName = "",
                         [CallerFilePath] string filePath = "",
                         [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Warn, message, argument, exception)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        public void Warn(string message,
                         object[] argument,
                         [CallerMemberName] string memberName = "",
                         [CallerFilePath] string filePath = "",
                         [CallerLineNumber] int lineNumber = 0)
        {
            this.Write(LogEventInfoBuilder.Create(LogLevel.Warn, message, argument)
                                          .WithCategory(this.Name)
                                          .WithTimeStamp(DateTimeOffset.Now)
                                          .WithAssembly(_logFactory.Config.Assembly)
                                          .WithCallerInfo(memberName, filePath, lineNumber));
        }

        private string BuildLayout(LogEventInfo logEventInfo)
        {
            const string startSeparator = "${";
            const string endSeparator = "}";

            var result = new StringBuilder();
            var logLayout = _logFactory.GetLogLayouts();
            var layout = _logFactory.Config.Layout;
            var startIndex = layout.IndexOf(startSeparator, StringComparison.Ordinal);
            var endIndex = layout.IndexOf(endSeparator, StringComparison.Ordinal);
            while (startIndex > 0 && endIndex > 0)
            {
                result.Append(layout.Substring(0, startIndex));

                var layoutVariable = layout.Substring(startIndex, endIndex - startIndex + 1);
                var layoutArgs = layoutVariable.TrimStart(startSeparator.ToCharArray()).TrimEnd(endSeparator.ToCharArray()).Split(':');
                var layoutName = layoutArgs[0];
                var layoutFormat = layoutArgs.Length > 1 ? layoutArgs[1] : string.Empty;

                var nameParts = layoutName.Split(',');
                layoutName = nameParts[0];
                int? width = null;
                if (nameParts.Length > 1 && int.TryParse(nameParts[1], out var parsed))
                {
                    width = parsed;
                }

                if (logLayout.ContainsKey(layoutName))
                {
                    var sbBefore = result.Length;
                    logLayout[layoutName].Append(result, logEventInfo, layoutFormat);
                    if (width.HasValue)
                    {
                        var inserted = result.ToString(sbBefore, result.Length - sbBefore);
                        result.Length = sbBefore;
                        result.Append(width.Value < 0 ? inserted.PadRight(Math.Abs(width.Value)) : inserted.PadLeft(width.Value));
                    }
                }
                else
                {
                    result.Append(layoutVariable);
                }

                layout = layout.Substring(endIndex + 1);
                startIndex = layout.IndexOf(startSeparator, StringComparison.Ordinal);
                endIndex = layout.IndexOf(endSeparator, StringComparison.Ordinal);
            }

            return result.ToString();
        }

        private void Write(LogEventInfo logEventInfo)
        {
            if (logEventInfo.Level < _logFactory.Config.MinLogLevel)
            {
                return;
            }

            var line = this.BuildLayout(logEventInfo);

            if (_logFactory.Config.IncludeLogOriginDetails)
            {
                line += $" > [FileName: {Path.GetFileName(logEventInfo.CallerFilePath)}, LineNumber: {logEventInfo.CallerLineNumber}, Origin: {logEventInfo.CallerMemberName}()]";
            }

            foreach (var logWriter in _logFactory.Config.LogWriters)
            {
                logWriter.Write(line);
            }
        }
    }
}