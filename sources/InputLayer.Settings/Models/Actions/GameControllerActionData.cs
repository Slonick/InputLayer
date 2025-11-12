using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;
using InputLayer.Settings.Models.Actions.Settings;

namespace InputLayer.Settings.Models.Actions
{
    [XmlType("GameControllerAction", Namespace = "clr-namespace:InputLayer.Settings")]
    public class GameControllerActionData : ActionDataBase
    {
        [XmlAttribute("ActionType")]
        public GameControllerActionType ActionType { get; set; }

        [XmlElement("Settings")]
        public GameControllerActionSettingsDataBase Settings { get; set; }

        public bool ShouldSerializeSettings()
            => this.Settings != null;
    }
}