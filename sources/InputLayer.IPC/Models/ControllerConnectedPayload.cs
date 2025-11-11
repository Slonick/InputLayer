namespace InputLayer.IPC.Models
{
    public class ControllerConnectedMessage : IPCMessage
    {
        /// <inheritdoc/>
        public override string ToString()
            => "Controller connected";
    }
}