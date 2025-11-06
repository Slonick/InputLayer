namespace InputLayer.IPC.Models
{
    public class ControllerConnectedMessage : IIPCMessage
    {
        /// <inheritdoc/>
        public override string ToString() => "Controller connected";
    }
}