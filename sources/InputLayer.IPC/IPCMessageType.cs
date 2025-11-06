namespace InputLayer.IPC
{
    public enum IPCMessageType
    {
        Ping = 1 << 1,
        ButtonPressed = 1 << 2,
        ButtonReleased = 1 << 3,
        ControllerConnected = 1 << 4,
        ControllerDisconnected = 1 << 5,
        Rumble = 1 << 6
    }
}