using System.Xml.Serialization;

namespace InputLayer.Settings.Models.Actions
{
    [XmlType("ExecutableAction", Namespace = "clr-namespace:InputLayer.Settings")]
    public class ExecutableActionData : ActionDataBase
    {
        [XmlElement("FileName")]
        public string FileName { get; set; }

        [XmlElement("Arguments")]
        public string Arguments { get; set; }

        [XmlAttribute("IsHidden")]
        public bool IsHidden { get; set; }

        [XmlElement("WorkingDirectory")]
        public string WorkingDirectory { get; set; }

        public bool ShouldSerializeArguments()
            => !string.IsNullOrEmpty(this.Arguments);

        public bool ShouldSerializeWorkingDirectory()
            => !string.IsNullOrEmpty(this.WorkingDirectory);
    }
}