using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Logging;
using static InputLayer.Agent.SDL;

namespace InputLayer.Agent
{
    public class SDLService : IDisposable
    {
        private const int TriggerThreshold = 32767 / 4;
        private readonly List<Controller> _controllers = new List<Controller>();
        private readonly object _controllersLock = new object();
        private readonly Thread _eventLoopThread;
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private bool _running;

        public SDLService()
        {
            _logger.Info("Initializing ..");

            SDL_GetVersion(out var version);
            _logger.Info($"SDL Version: {version.major}.{version.minor}.{version.patch}");

            SDL_SetHint(SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS, "1");
            SDL_SetHint(SDL_HINT_ACCELEROMETER_AS_JOYSTICK, "0");
            SDL_SetHint(SDL_HINT_MAC_BACKGROUND_APP, "1");
            SDL_SetHint(SDL_HINT_XINPUT_ENABLED, "1");
            // rawinput is broken for some reason with DPAD buttons
            SDL_SetHint(SDL_HINT_JOYSTICK_RAWINPUT, "0");

            if (SDL_Init(SDL_INIT_GAMECONTROLLER | SDL_INIT_JOYSTICK | SDL_INIT_EVENTS | SDL_INIT_HAPTIC) < 0)
            {
                var error = SDL_GetError();
                _logger.Fatal($"SDL init failed: {error}");
                throw new Exception("SDL init failed: " + error);
            }

            _logger.Info("SDL initialized successfully.");

            _running = true;
            _eventLoopThread = new Thread(this.EventLoop) { IsBackground = true };
            _eventLoopThread.Start();
            _logger.Info("SDL event loop thread started.");
        }

        public event Action<ControllerInput> ButtonPressed;

        public event Action<ControllerInput> ButtonReleased;

        public event Action ControllerConnected;

        public event Action ControllerDisconnected;

        public void Dispose()
        {
            if (!_running)
            {
                return;
            }

            _logger.Info("Disposing SDLService...");

            _running = false;
            _eventLoopThread?.Join();

            lock (_controllersLock)
            {
                foreach (var controller in _controllers)
                {
                    SDL_GameControllerClose(controller.ControllerId);
                }
            }

            SDL_Quit();
            _logger.Info("SDLService disposed.");
        }

        public void Rumble(int durationMs = 200, float intensity = 0.5f)
        {
            lock (_controllersLock)
            {
                if (!_controllers.Any())
                {
                    _logger.Warn("Cannot rumble - no controller connected.");
                    return;
                }
            }

            try
            {
                _logger.Debug($"Triggering rumble: duration={durationMs}ms, intensity={intensity}");

                var lowFreq = (ushort)(ushort.MaxValue * intensity);
                var highFreq = (ushort)(ushort.MaxValue * intensity);

                lock (_controllersLock)
                {
                    foreach (var controller in _controllers)
                    {
                        var result = SDL_GameControllerRumble(controller.ControllerId, lowFreq, highFreq, (uint)durationMs);

                        if (result != 0)
                        {
                            _logger.Warn($"Failed to trigger vibration: {SDL_GetError()}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error triggering vibration.");
            }
        }

        private void CheckConnectedControllersImmediately()
        {
            try
            {
                var joystickCount = SDL_NumJoysticks();
                for (var i = 0; i < joystickCount; i++)
                {
                    this.HandleControllerAdded(i);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during immediate controller scan");
            }
        }

        private void EventLoop()
        {
            _logger.Info("SDL event loop started.");

            this.CheckConnectedControllersImmediately();

            try
            {
                while (_running)
                {
                    while (SDL_PollEvent(out var e) == 1)
                    {
                        switch (e.type)
                        {
                            case SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                                _logger.Debug($"Controller device added event: device index {e.cdevice.which}");
                                this.HandleControllerAdded(e.cdevice.which);
                                break;
                            case SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                                _logger.Debug($"Controller device removed event: instance ID {e.cdevice.which}");
                                this.HandleControllerRemoved(e.cdevice.which);
                                break;
                        }
                    }

                    this.ProcessInputs();

                    Thread.Sleep(16);
                }

                _logger.Info("SDL event loop stopped.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception in GamepadService loop.");
            }
        }

        private void HandleControllerAdded(int deviceIndex)
        {
            _logger.Info($"Attempting to open controller at device index {deviceIndex}...");

            if (SDL_IsGameController(deviceIndex) == SDL_bool.SDL_FALSE)
            {
                _logger.Warn($"Device at index {deviceIndex} is not a supported game controller");
                return;
            }

            var controllerId = SDL_GameControllerOpen(deviceIndex);
            if (controllerId == IntPtr.Zero)
            {
                _logger.Error($"Failed to open controller at index {deviceIndex}: {SDL_GetError()}");
                return;
            }

            lock (_controllersLock)
            {
                if (_controllers.Any(x => x.ControllerId == controllerId))
                {
                    _logger.Warn($"Controller at index {deviceIndex} is already connected");
                    return;
                }

                var joystickId = SDL_GameControllerGetJoystick(controllerId);
                var controller = new Controller(controllerId, SDL_JoystickInstanceID(joystickId), SDL_JoystickName(joystickId));

                _controllers.Add(controller);

                _logger.Info($"added controller index {controller.InstanceId}, {controller.Name}");
                this.ControllerConnected?.Invoke();
            }
        }

        private void HandleControllerRemoved(int instanceId)
        {
            lock (_controllersLock)
            {
                var controller = _controllers.SingleOrDefault(x => x.InstanceId == instanceId);
                if (controller is null)
                {
                    _logger.Debug("No controller connected. Ignoring removal event.");
                    return;
                }

                SDL_GameControllerClose(controller.ControllerId);
                _controllers.Remove(controller);
                this.ControllerDisconnected?.Invoke();
            }
        }

        private void ProcessAxisState(short currentState, ControllerInput button, Controller controller)
        {
            var pressed = currentState > TriggerThreshold;
            this.ProcessEvent(pressed, button, controller);
        }

        private void ProcessButtonState(byte currentState, ControllerInput button, Controller controller)
        {
            var pressed = currentState == 1;
            this.ProcessEvent(pressed, button, controller);
        }

        private void ProcessEvent(bool pressed, ControllerInput button, Controller controller)
        {
            if (pressed && controller.LastInputState[button] == ControllerInputState.Released)
            {
                _logger.Debug($"Button {button} pressed");
                controller.LastInputState[button] = ControllerInputState.Pressed;
                this.ButtonPressed?.Invoke(button);
            }
            else if (!pressed && controller.LastInputState[button] == ControllerInputState.Pressed)
            {
                _logger.Debug($"Button {button} released");
                controller.LastInputState[button] = ControllerInputState.Released;
                this.ButtonReleased?.Invoke(button);
            }
        }

        private void ProcessInputs()
        {
            if (!_running)
            {
                return;
            }

            SDL_GameControllerUpdate();
            lock (_controllersLock)
            {
                foreach (var controller in _controllers)
                {
                    this.ProcessState(controller);
                }
            }
        }

        private void ProcessState(Controller controller)
        {
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A), ControllerInput.A, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B), ControllerInput.B, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK), ControllerInput.Back, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE), ControllerInput.Guide, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER), ControllerInput.LeftShoulder, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSTICK), ControllerInput.LeftStick, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER), ControllerInput.RightShoulder, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSTICK), ControllerInput.RightStick, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START), ControllerInput.Start, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X), ControllerInput.X, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y), ControllerInput.Y, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN), ControllerInput.DPadDown, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT), ControllerInput.DPadLeft, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT), ControllerInput.DPadRight, controller);
            this.ProcessButtonState(SDL_GameControllerGetButton(controller.ControllerId, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP), ControllerInput.DPadUp, controller);

            this.ProcessAxisState(SDL_GameControllerGetAxis(controller.ControllerId, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT), ControllerInput.TriggerLeft, controller);
            this.ProcessAxisState(SDL_GameControllerGetAxis(controller.ControllerId, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT), ControllerInput.TriggerRight, controller);
        }
    }
}