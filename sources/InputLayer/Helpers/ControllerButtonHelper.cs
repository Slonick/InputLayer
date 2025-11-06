using System;
using System.Collections.Generic;
using System.Linq;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Helpers
{
    public static class ControllerButtonHelper
    {
        public static List<ControllerInput> All { get; } = Enum.GetValues(typeof(ControllerInput))
                                                               .Cast<ControllerInput>()
                                                               .Where(x => x != ControllerInput.None &&
                                                                           x != ControllerInput.Guide &&
                                                                           x != ControllerInput.LeftStickDown &&
                                                                           x != ControllerInput.LeftStickLeft &&
                                                                           x != ControllerInput.LeftStickRight &&
                                                                           x != ControllerInput.LeftStickUp &&
                                                                           x != ControllerInput.RightStickDown &&
                                                                           x != ControllerInput.RightStickLeft &&
                                                                           x != ControllerInput.RightStickRight &&
                                                                           x != ControllerInput.RightStickUp)
                                                               .ToList();

        public static List<ControllerInput> MainButtons => new List<ControllerInput>
        {
            ControllerInput.Guide,
            ControllerInput.Start,
            ControllerInput.Back,
        };

        public static string GetDisplayName(ControllerInput button)
        {
            switch (button)
            {
                case ControllerInput.A:
                    return "A / Cross ✖";
                case ControllerInput.B:
                    return "B / Circle ●";
                case ControllerInput.X:
                    return "X / Square ■";
                case ControllerInput.Y:
                    return "Y / Triangle ▲";
                case ControllerInput.Guide:
                    return "Guide / PS";
                case ControllerInput.Start:
                    return "Start / Options";
                case ControllerInput.Back:
                    return "Back / Share";
                case ControllerInput.LeftStick:
                    return "LS / L3";
                case ControllerInput.RightStick:
                    return "RS / R3";
                case ControllerInput.LeftShoulder:
                    return "LB / L1";
                case ControllerInput.RightShoulder:
                    return "RB / R1";
                case ControllerInput.TriggerLeft:
                    return "LT / L2";
                case ControllerInput.TriggerRight:
                    return "RT / R2";
                case ControllerInput.DPadUp:
                    return "Up";
                case ControllerInput.DPadDown:
                    return "Down";
                case ControllerInput.DPadLeft:
                    return "Left";
                case ControllerInput.DPadRight:
                    return "Right";
                default:
                    return button.ToString();
            }
        }
    }
}