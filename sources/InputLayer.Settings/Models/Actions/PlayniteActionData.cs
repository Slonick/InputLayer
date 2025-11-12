using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Settings.Models.Actions
{
    [XmlType("PlayniteAction", Namespace = "clr-namespace:InputLayer.Settings")]
    public class PlayniteActionData : ActionDataBase
    {
        [XmlAttribute("ActionType")]
        public PlayniteActionType ActionType { get; set; }
    }
}