namespace InputLayer.IPC.Models
{
    public class PingMessage : IPCMessage
    {
        /// <inheritdoc/>
        public override string ToString()
            => "Ping";
    }
}