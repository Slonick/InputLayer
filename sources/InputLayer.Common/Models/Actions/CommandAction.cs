using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using InputLayer.Common.Logging;

namespace InputLayer.Common.Models.Actions
{
    public class CommandAction : ObservableObject, IExecutableAction
    {
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        private string _command;
        private bool _isHidden = true;

        public string Command
        {
            get => _command;
            set => this.SetValue(ref _command, value);
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
                    FileName = "cmd.exe",
                    Arguments = $"/c {this.Command}",
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
                        _logger.Error("Failed to start CMD process for command: " + this.Command);
                        return;
                    }

                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    var exited = process.WaitForExit(5000);

                    if (!exited)
                    {
                        _logger.Warn($"Process timeout after {5000}ms, killing process");
                        process.Kill();
                        process.WaitForExit();
                    }

                    _logger.Info($"CMD command executed: {this.Command}");
                    _logger.Info($"Exit code: {process.ExitCode}");

                    if (!string.IsNullOrEmpty(output))
                    {
                        _logger.Info($"CMD output: {output}");
                    }
                    else
                    {
                        _logger.Info("CMD output: [no output]");
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.Warn($"CMD errors: {error}");
                    }

                    if (process.ExitCode != 0)
                    {
                        _logger.Warn($"CMD command completed with non-zero exit code: {process.ExitCode}");
                    }
                    else
                    {
                        _logger.Info("CMD command completed successfully");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error executing command: " + this.Command);
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"Command: {this.Command}";
    }
}