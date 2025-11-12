using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions.Settings
{
    [XmlInclude(typeof(SystemActionPauseSettingsData))]
    [XmlType("SystemActionSettings", Namespace = "clr-namespace:InputLayer.Settings")]
    public abstract class SystemActionSettingsDataBase { }
}