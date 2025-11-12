using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;

namespace InputLayer.IPC.Models
{
    public class ButtonPressedMessage : IPCMessage
    {
        public ButtonPressedMessage() { }

        public ButtonPressedMessage(ControllerInput button)
        {
            this.Button = button;
        }

        [XmlAttribute]
        public ControllerInput Button { get; set; }

        /// <inheritdoc/>
        public override string ToString()
            => $"Button pressed: {this.Button}";
    }
}