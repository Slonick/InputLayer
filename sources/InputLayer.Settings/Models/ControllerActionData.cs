using System.Collections.Generic;
using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Settings.Models
{
    [XmlType("ControllerAction")]
    public class ControllerActionData
    {
        [XmlArray("Actions")]
        [XmlArrayItem("ControllerActionItem")]
        public List<ControllerActionItemData> Actions { get; set; } = new List<ControllerActionItemData>();

        [XmlAttribute("Button")]
        public ControllerInput Button { get; set; }

        [XmlAttribute("IsPredefined")]
        public bool IsPredefined { get; set; }

        [XmlAttribute("Mode")]
        public ControllerButtonMode Mode { get; set; }
    }
}