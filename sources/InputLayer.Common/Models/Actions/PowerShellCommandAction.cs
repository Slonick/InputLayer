using System;
using System.Collections.Generic;
using System.Diagnostics;
using InputLayer.Common.Logging;

namespace InputLayer.Common.Models.Actions
{
    public class PowerShellCommandAction : ObservableObject, IExecutableAction
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
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{this.Command}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = this.IsHidden,
                    WindowStyle = this.IsHidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        _logger.Error("Failed to start PowerShell process");
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

                    _logger.Info($"PowerShell command executed: {this.Command}");

                    if (!string.IsNullOrEmpty(output))
                    {
                        _logger.Info($"PowerShell output: {output}");
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.Error($"PowerShell errors: {error}");
                    }

                    _logger.Info($"PowerShell process exited with code: {process.ExitCode}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error executing command: " + this.Command);
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"PowerShell Command: {this.Command}";
    }
}