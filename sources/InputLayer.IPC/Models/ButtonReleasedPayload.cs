using InputLayer.Common.Infrastructures;

namespace InputLayer.IPC.Models
{
    public class ButtonReleasedMessage : IIPCMessage
    {
        public ButtonReleasedMessage() { }

        public ButtonReleasedMessage(ControllerInput button)
        {
            this.Button = button;
        }

        public ControllerInput Button { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"Button released: {this.Button}";
    }
}