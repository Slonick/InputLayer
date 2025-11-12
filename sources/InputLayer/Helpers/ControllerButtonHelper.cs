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
                                                                           x != ControllerInput.RightStickUp &&
                                                                           x != ControllerInput.Main)
                                                               .ToList();

        public static List<ControllerInput> MainButtons
            => new List<ControllerInput>
            {
                ControllerInput.Guide,
                ControllerInput.Start,
                ControllerInput.Back
            };

        public static string GetDisplayName(ControllerInput button, ControllerInputDisplayMode mode = ControllerInputDisplayMode.XBOX)
        {
            switch (button)
            {
                case ControllerInput.A:
                    return MapButton(mode, "A", "Cross ✖", "B");
                case ControllerInput.B:
                    return MapButton(mode, "B", "Circle ●", "A");
                case ControllerInput.X:
                    return MapButton(mode, "X", "Square ■", "Y");
                case ControllerInput.Y:
                    return MapButton(mode, "Y", "Triangle ▲", "X");
                case ControllerInput.Guide:
                    return MapButton(mode, "Guide", "PS", "Home");
                case ControllerInput.Start:
                    return MapButton(mode, "Start", "Options", "Plus +");
                case ControllerInput.Back:
                    return MapButton(mode, "Back", "Share", "Minus −");
                case ControllerInput.LeftStick:
                    return MapButton(mode, "LS", "L3", "Left Stick");
                case ControllerInput.RightStick:
                    return MapButton(mode, "RS", "R3", "Right Stick");
                case ControllerInput.LeftShoulder:
                    return MapButton(mode, "LB", "L1", "L");
                case ControllerInput.RightShoulder:
                    return MapButton(mode, "RB", "R1", "R");
                case ControllerInput.TriggerLeft:
                    return MapButton(mode, "LT", "L2", "ZL");
                case ControllerInput.TriggerRight:
                    return MapButton(mode, "RT", "R2", "ZR");
                case ControllerInput.DPadUp:
                    return "D-Pad Up";
                case ControllerInput.DPadDown:
                    return "D-Pad Down";
                case ControllerInput.DPadLeft:
                    return "D-Pad Left";
                case ControllerInput.DPadRight:
                    return "D-Pad Right";
                default:
                    return button.ToString();
            }
        }

        private static string MapButton(ControllerInputDisplayMode mode, string xboxButton, string psButton, string switchButton)
        {
            switch (mode)
            {
                case ControllerInputDisplayMode.PS:
                    return psButton;
                case ControllerInputDisplayMode.NintendoSwitch:
                    return switchButton;
                case ControllerInputDisplayMode.XBOX:
                default:
                    return xboxButton;
            }
        }
    }
}