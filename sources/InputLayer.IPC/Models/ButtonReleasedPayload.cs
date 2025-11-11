using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;

namespace InputLayer.IPC.Models
{
    public class ButtonReleasedMessage : IPCMessage
    {
        public ButtonReleasedMessage() { }

        public ButtonReleasedMessage(ControllerInput button)
        {
            this.Button = button;
        }

        [XmlAttribute]
        public ControllerInput Button { get; set; }

        /// <inheritdoc/>
        public override string ToString()
            => $"Button released: {this.Button}";
    }
}