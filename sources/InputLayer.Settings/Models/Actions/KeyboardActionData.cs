using System.Xml.Serialization;
using InputLayer.Keyboard;

namespace InputLayer.Settings.Models.Actions
{
    [XmlType("KeyboardAction", Namespace = "clr-namespace:InputLayer.Settings")]
    public class KeyboardActionData : ActionDataBase
    {
        [XmlAttribute("Key")]
        public Keys Key { get; set; }

        [XmlArray("Modifiers")]
        [XmlArrayItem("Modifier")]
        public Modifiers[] Modifiers { get; set; }

        public bool ShouldSerializeModifiers()
            => this.Modifiers != null && this.Modifiers.Length > 0;
    }
}