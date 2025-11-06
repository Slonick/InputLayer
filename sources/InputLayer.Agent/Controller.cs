using System;
using System.Collections.Generic;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Agent
{
    public class Controller
    {
        public readonly Dictionary<ControllerInput, ControllerInputState> LastInputState = new Dictionary<ControllerInput, ControllerInputState>
        {
            { ControllerInput.A, ControllerInputState.Released },
            { ControllerInput.B, ControllerInputState.Released },
            { ControllerInput.Back, ControllerInputState.Released },
            { ControllerInput.DPadDown, ControllerInputState.Released },
            { ControllerInput.DPadLeft, ControllerInputState.Released },
            { ControllerInput.DPadRight, ControllerInputState.Released },
            { ControllerInput.DPadUp, ControllerInputState.Released },
            { ControllerInput.Guide, ControllerInputState.Released },
            { ControllerInput.LeftShoulder, ControllerInputState.Released },
            { ControllerInput.LeftStick, ControllerInputState.Released },
            { ControllerInput.LeftStickDown, ControllerInputState.Released },
            { ControllerInput.LeftStickLeft, ControllerInputState.Released },
            { ControllerInput.LeftStickRight, ControllerInputState.Released },
            { ControllerInput.LeftStickUp, ControllerInputState.Released },
            { ControllerInput.RightShoulder, ControllerInputState.Released },
            { ControllerInput.RightStick, ControllerInputState.Released },
            { ControllerInput.RightStickDown, ControllerInputState.Released },
            { ControllerInput.RightStickLeft, ControllerInputState.Released },
            { ControllerInput.RightStickRight, ControllerInputState.Released },
            { ControllerInput.RightStickUp, ControllerInputState.Released },
            { ControllerInput.Start, ControllerInputState.Released },
            { ControllerInput.TriggerLeft, ControllerInputState.Released },
            { ControllerInput.TriggerRight, ControllerInputState.Released },
            { ControllerInput.X, ControllerInputState.Released },
            { ControllerInput.Y, ControllerInputState.Released }
        };

        public Controller(IntPtr controllerId, IntPtr joystickId, int instanceId, string name)
        {
            this.ControllerId = controllerId;
            this.JoystickId = joystickId;
            this.InstanceId = instanceId;
            this.Name = name;
        }

        public IntPtr ControllerId { get; }
        public int InstanceId { get; }
        public IntPtr JoystickId { get; }
        public string Name { get; }
    }
}