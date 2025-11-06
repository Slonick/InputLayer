using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InputLayer.Common.Constants;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Logging;
using InputLayer.Common.Services;
using InputLayer.IPC;
using InputLayer.IPC.Models;
using Timer = System.Timers.Timer;

namespace InputLayer.Services
{
    public sealed class ControllerService : IControllerService
    {
        private const int LongPressDurationMs = 2000;
        private const int SinglePressDelayMs = 500;

        private readonly object _buttonLock = new object();
        private readonly HashSet<ControllerInput> _combinationButtons = new HashSet<ControllerInput>();
        private readonly IPCClient _ipcClient;
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private readonly Dictionary<ControllerInput, Timer> _longPressTimers = new Dictionary<ControllerInput, Timer>();
        private readonly HashSet<ControllerInput> _pressedButtons = new HashSet<ControllerInput>();
        private readonly Dictionary<ControllerInput, Timer> _singlePressTimers = new Dictionary<ControllerInput, Timer>();

        private bool _combinationMode;
        private bool _isRunning;

        public ControllerService()
        {
            _ipcClient = new IPCClient();
            _ipcClient.MessageReceived += this.OnMessageReceived;
            _ipcClient.Disconnected += this.OnDisconnected;
        }

        public event Action<IReadOnlyList<ControllerInput>> ButtonCombinationPressed;
        public event Action<IReadOnlyList<ControllerInput>> ButtonCombinationReleased;
        public event Action<ControllerInput> ButtonPressed;
        public event Action<ControllerInput> ButtonReleased;

        public void Dispose()
        {
            _isRunning = false;

            this.StopAgent();

            this.StopAllSinglePressTimers();
            this.StopAllLongPressTimers();

            if (_ipcClient != null)
            {
                _ipcClient.MessageReceived -= this.OnMessageReceived;
                _ipcClient.Disconnected -= this.OnDisconnected;
                _ipcClient.Dispose();
            }
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            _logger.Info("Initialize.");
            this.StopAgent();
            this.StartAgent();

            _ipcClient.Connect();
            _logger.Info("Initialized successfully.");
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            _logger.Info("Initialize.");
            this.StopAgent();
            this.StartAgent();

            await _ipcClient.ConnectAsync(CancellationToken.None);
            _logger.Info("Initialized successfully.");
        }

        public void Rumble(int durationMs = 200, float intensity = 0.5f)
        {
            _logger.Debug($"Rumble: duration={durationMs}, intensity={intensity}");
            _ipcClient.Send(new RumbleMessage(durationMs, intensity));
        }

        private void HandleButtonPressed(ControllerInput button)
        {
            lock (_buttonLock)
            {
                _logger.Debug($"Button pressed: {button}");
                _pressedButtons.Add(button);
                _logger.Debug($"Buttons state: {string.Join(" | ", _pressedButtons)} ({_pressedButtons.Count})");

                if (_pressedButtons.Count == 1)
                {
                    this.StartSinglePressTimer(button);
                    this.StartLongPressTimer(button);
                }
                else if (_pressedButtons.Count == 2)
                {
                    _combinationMode = true;
                    _logger.Debug("Button combination pressed");

                    this.StopAllSinglePressTimers();
                    this.StopAllLongPressTimers();

                    foreach (var pressedButton in _pressedButtons)
                    {
                        _combinationButtons.Add(pressedButton);
                    }

                    this.ButtonCombinationPressed?.Invoke(_combinationButtons.ToArray());
                }
            }
        }

        private void HandleButtonReleased(ControllerInput button)
        {
            lock (_buttonLock)
            {
                _logger.Debug($"Button released: {button}");
                _pressedButtons.Remove(button);
                _logger.Debug($"Buttons state: {string.Join(" | ", _pressedButtons)}");

                this.StopSinglePressTimer(button);
                this.StopLongPressTimer(button);

                if (_combinationMode)
                {
                    if (_pressedButtons.Count == 0)
                    {
                        this.ButtonCombinationReleased?.Invoke(_combinationButtons.ToArray());
                        _combinationButtons.Clear();
                        _combinationMode = false;
                    }
                }
                else
                {
                    this.ButtonReleased?.Invoke(button);
                }
            }
        }

        private void OnDisconnected()
        {
            if (_isRunning)
            {
                _logger.Info("IPC connection lost, attempting to restart...");
                this.Initialize();
            }
        }

        private void OnLongPressTimerElapsed(ControllerInput button)
        {
            lock (_buttonLock)
            {
                if (_pressedButtons.Contains(button))
                {
                    _combinationMode = true;
                    _logger.Debug($"Long press: {button}");
                    var longPressButton = new[] { button, ControllerInput.LongPress };
                    this.ButtonCombinationPressed?.Invoke(longPressButton);
                }

                _longPressTimers.Remove(button);
            }
        }

        private void OnMessageReceived(IIPCMessage message)
        {
            switch (message)
            {
                case ButtonPressedMessage payload:
                    _logger.Debug($"Button pressed: {payload.Button}");
                    this.HandleButtonPressed(payload.Button);
                    break;
                case ButtonReleasedMessage payload:
                    _logger.Debug($"Button released: {payload.Button}");
                    this.HandleButtonReleased(payload.Button);
                    break;
                case ControllerConnectedMessage _:
                    _logger.Info("Controller connected");
                    break;
                case ControllerDisconnectedMessage _:
                    _logger.Info("Controller disconnected");
                    break;
                case RumbleMessage _:
                    break;
                case PingMessage _:
                    _ipcClient.Send(new PingMessage());
                    break;
            }
        }

        private void OnSinglePressTimerElapsed(ControllerInput button)
        {
            lock (_buttonLock)
            {
                if (_pressedButtons.Contains(button))
                {
                    _logger.Debug($"Single button press: {button}");
                    this.ButtonPressed?.Invoke(button);
                }

                _singlePressTimers.Remove(button);
            }
        }

        private void StartAgent()
        {
            try
            {
                if (!File.Exists(PathConstants.AgentFile))
                {
                    _logger.Error($"Agent not found: {PathConstants.AgentFile}");
                    return;
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = PathConstants.AgentFile,
                    UseShellExecute = false,
                    #if !DEBUG
                    CreateNoWindow = true
                    #endif
                };

                Process.Start(startInfo);
                _logger.Info("Agent started");
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error starting agent");
            }
        }

        private void StartLongPressTimer(ControllerInput button)
        {
            var timer = new Timer(LongPressDurationMs);
            timer.Elapsed += (sender, args) => this.OnLongPressTimerElapsed(button);
            timer.AutoReset = false;
            timer.Start();
            _longPressTimers[button] = timer;
        }

        private void StartSinglePressTimer(ControllerInput button)
        {
            var timer = new Timer(SinglePressDelayMs);
            timer.Elapsed += (sender, args) => this.OnSinglePressTimerElapsed(button);
            timer.AutoReset = false;
            timer.Start();
            _singlePressTimers[button] = timer;
        }

        private void StopAgent()
        {
            try
            {
                foreach (var process in Process.GetProcessesByName("InputLayer.Agent"))
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(3000);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error stopping agent");
            }
        }

        private void StopAllLongPressTimers()
        {
            lock (_buttonLock)
            {
                foreach (var timer in _longPressTimers.Values)
                {
                    timer.Dispose();
                }

                _longPressTimers.Clear();
            }
        }

        private void StopAllSinglePressTimers()
        {
            lock (_buttonLock)
            {
                foreach (var timer in _singlePressTimers.Values)
                {
                    timer.Dispose();
                }

                _singlePressTimers.Clear();
            }
        }

        private void StopLongPressTimer(ControllerInput button)
        {
            if (_longPressTimers.TryGetValue(button, out var timer))
            {
                timer.Dispose();
                _longPressTimers.Remove(button);
            }
        }

        private void StopSinglePressTimer(ControllerInput button)
        {
            if (_singlePressTimers.TryGetValue(button, out var timer))
            {
                timer.Dispose();
                _singlePressTimers.Remove(button);
            }
        }
    }
}