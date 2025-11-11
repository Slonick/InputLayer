using System.Xml.Serialization;

namespace InputLayer.IPC.Models
{
    [XmlInclude(typeof(ButtonPressedMessage))]
    [XmlInclude(typeof(ButtonReleasedMessage))]
    [XmlInclude(typeof(ControllerConnectedMessage))]
    [XmlInclude(typeof(ControllerDisconnectedMessage))]
    [XmlInclude(typeof(RumbleMessage))]
    [XmlInclude(typeof(PingMessage))]
    [XmlType("IPCMessage", Namespace = "clr-namespace:InputLayer.IPC")]
    public abstract class IPCMessage { }
}