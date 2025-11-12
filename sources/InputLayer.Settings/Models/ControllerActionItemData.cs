using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;
using InputLayer.Settings.Models.Actions;

namespace InputLayer.Settings.Models
{
    [XmlType("ControllerActionItem")]
    public class ControllerActionItemData
    {
        [XmlElement("Action", Namespace = "clr-namespace:InputLayer.Settings")]
        public ActionDataBase Action { get; set; }

        [XmlAttribute("ActionType")]
        public ActionType ActionType { get; set; }
    }
}