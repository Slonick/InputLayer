using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions.Settings
{
    [XmlType("SystemActionPauseSettings", Namespace = "clr-namespace:InputLayer.Settings")]
    public class SystemActionPauseSettingsData : SystemActionSettingsDataBase
    {
        [XmlAttribute("Timeout")]
        public int Timeout { get; set; }
    }
}