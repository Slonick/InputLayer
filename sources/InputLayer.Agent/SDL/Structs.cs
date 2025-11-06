using System.Runtime.InteropServices;

// ReSharper disable All
namespace InputLayer.Agent
{
    #region SDL_version

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_version
    {
        public byte major;
        public byte minor;
        public byte patch;
    }

    #endregion

    #region SDL_ControllerDeviceEvent

    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_ControllerDeviceEvent
    {
        public SDL_EventType type;
        public uint timestamp;
        public int which;
    }

    #endregion

    #region SDL_Event

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct SDL_Event
    {
        [FieldOffset(0)]
        public SDL_EventType type;

        [FieldOffset(0)]
        public SDL_ControllerDeviceEvent cdevice;

        // Padding to ensure correct size (56 bytes on most platforms)
        [FieldOffset(0)]
        private fixed byte padding[56];
    }

    #endregion
}