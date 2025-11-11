using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions.Settings
{
    [XmlInclude(typeof(GameControllerActionRumbleSettingsData))]
    [XmlType("GameControllerActionSettings", Namespace = "clr-namespace:InputLayer.Settings")]
    public abstract class GameControllerActionSettingsDataBase { }
}