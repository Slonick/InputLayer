using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;
using InputLayer.Settings.Models.Actions.Settings;

namespace InputLayer.Settings.Models.Actions
{
    [XmlType("SystemAction", Namespace = "clr-namespace:InputLayer.Settings")]
    public class SystemActionData : ActionDataBase
    {
        [XmlAttribute("ActionType")]
        public SystemActionType ActionType { get; set; }

        [XmlElement("Settings")]
        public SystemActionSettingsDataBase Settings { get; set; }

        public bool ShouldSerializeSettings()
            => this.Settings != null;
    }
}