using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions
{
    [XmlInclude(typeof(CommandActionData))]
    [XmlInclude(typeof(PowerShellCommandActionData))]
    [XmlInclude(typeof(ExecutableActionData))]
    [XmlInclude(typeof(KeyboardActionData))]
    [XmlInclude(typeof(PlayniteActionData))]
    [XmlInclude(typeof(GameControllerActionData))]
    [XmlInclude(typeof(SystemActionData))]
    [XmlType("Action", Namespace = "clr-namespace:InputLayer.Settings")]
    public abstract class ActionDataBase { }
}