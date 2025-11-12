using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions.Settings
{
    [XmlType("GameControllerActionRumbleSettings", Namespace = "clr-namespace:InputLayer.Settings")]
    public class GameControllerActionRumbleSettingsData : GameControllerActionSettingsDataBase
    {
        [XmlAttribute("DurationMs")]
        public int DurationMs { get; set; }

        [XmlAttribute("Intensity")]
        public float Intensity { get; set; }
    }
}