namespace InputLayer.IPC.Models
{
    public class ControllerDisconnectedMessage : IIPCMessage
    {
        /// <inheritdoc/>
        public override string ToString() => "Controller disconnected";
    }
}