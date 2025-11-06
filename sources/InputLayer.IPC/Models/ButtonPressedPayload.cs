using InputLayer.Common.Infrastructures;

namespace InputLayer.IPC.Models
{
    public class ButtonPressedMessage : IIPCMessage
    {
        public ButtonPressedMessage() { }

        public ButtonPressedMessage(ControllerInput button)
        {
            this.Button = button;
        }

        public ControllerInput Button { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"Button pressed: {this.Button}";
    }
}