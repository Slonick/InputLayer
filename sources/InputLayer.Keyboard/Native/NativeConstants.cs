// ReSharper disable InconsistentNaming

namespace InputLayer.Keyboard.Native
{
    internal static class NativeConstants
    {
        internal const int INPUT_KEYBOARD = 1;
        internal const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        internal const uint KEYEVENTF_KEYUP = 0x0002;
        internal const uint KEYEVENTF_SCANCODE = 0x0008;

        internal const int WH_KEYBOARD_LL = 13;
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x0101;
        internal const int WM_SYSKEYDOWN = 0x0104;
        internal const int WM_SYSKEYUP = 0x0105;
    }
}