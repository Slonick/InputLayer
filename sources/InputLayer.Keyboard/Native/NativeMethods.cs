using System.Runtime.InteropServices;

namespace InputLayer.Keyboard.Native
{
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern ushort MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, NativeStructures.INPUT[] pInputs, int cbSize);
    }
}