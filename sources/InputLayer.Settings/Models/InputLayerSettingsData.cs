using System.Collections.Generic;
using System.Xml.Serialization;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Settings.Models
{
    [XmlRoot("InputLayerSettings", Namespace = "clr-namespace:InputLayer.Settings")]
    public class InputLayerSettingsData
    {
        [XmlArray("DesktopActions")]
        [XmlArrayItem("ControllerAction")]
        public List<ControllerActionData> DesktopActions { get; set; } = new List<ControllerActionData>();

        [XmlArray("FullScreenActions")]
        [XmlArrayItem("ControllerAction")]
        public List<ControllerActionData> FullScreenActions { get; set; } = new List<ControllerActionData>();

        [XmlArray("InGameActions")]
        [XmlArrayItem("ControllerAction")]
        public List<ControllerActionData> InGameActions { get; set; } = new List<ControllerActionData>();

        [XmlElement("MainButton")]
        public ControllerInput MainButton { get; set; }

        [XmlElement("DisplayMode")]
        public ControllerInputDisplayMode DisplayMode { get; set; }
    }
}