using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions
{
    [XmlType("PowerShellCommandAction", Namespace = "clr-namespace:InputLayer.Settings")]
    public class PowerShellCommandActionData : ActionDataBase
    {
        [XmlElement("Command")]
        public string Command { get; set; }

        [XmlAttribute("IsHidden")]
        public bool IsHidden { get; set; }

        [XmlElement("WorkingDirectory")]
        public string WorkingDirectory { get; set; }

        public bool ShouldSerializeWorkingDirectory()
            => !string.IsNullOrEmpty(this.WorkingDirectory);
    }
}