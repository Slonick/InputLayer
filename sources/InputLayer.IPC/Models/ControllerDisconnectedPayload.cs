namespace InputLayer.IPC.Models
{
    public class ControllerDisconnectedMessage : IPCMessage
    {
        /// <inheritdoc/>
        public override string ToString()
            => "Controller disconnected";
    }
}