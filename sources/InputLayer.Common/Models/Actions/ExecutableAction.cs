using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using InputLayer.Common.Logging;

namespace InputLayer.Common.Models.Actions
{
    public class ExecutableAction : ObservableObject, IExecutableAction
    {
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        private string _arguments;
        private string _fileName;
        private bool _isHidden = true;

        public string Arguments
        {
            get => _arguments;
            set => this.SetValue(ref _arguments, value);
        }

        public string FileName
        {
            get => _fileName;
            set => this.SetValue(ref _fileName, value);
        }

        public bool IsHidden
        {
            get => _isHidden;
            set => this.SetValue(ref _isHidden, value);
        }

        /// <inheritdoc/>
        public void Execute(object obj)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = this.FileName,
                    Arguments = this.Arguments,
                    UseShellExecute = false,
                    CreateNoWindow = this.IsHidden,
                    WindowStyle = this.IsHidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,

                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        _logger.Error($"Failed to start process: {this.FileName}");
                        return;
                    }

                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    var exited = process.WaitForExit(5000);

                    if (!exited)
                    {
                        _logger.Warn($"Process timeout after {5000}ms, killing process: {this.FileName}");
                        process.Kill();
                        process.WaitForExit();
                    }

                    _logger.Info($"Executed: {this.FileName} {this.Arguments}");
                    _logger.Info($"Process ID: {process.Id}");
                    _logger.Info($"Exit code: {process.ExitCode}");
                    _logger.Info($"Timeout: {!exited}");

                    if (!string.IsNullOrEmpty(output))
                    {
                        _logger.Info($"Process output: {output}");
                    }
                    else
                    {
                        _logger.Info("Process output: [no output]");
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.Warn($"Process errors: {error}");
                    }

                    if (process.ExitCode != 0)
                    {
                        _logger.Warn($"Process completed with non-zero exit code: {process.ExitCode}");
                    }
                    else
                    {
                        _logger.Info("Process completed successfully");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error executing: {this.FileName} {this.Arguments}");
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"Execute: {this.FileName} {this.Arguments}";
    }
}