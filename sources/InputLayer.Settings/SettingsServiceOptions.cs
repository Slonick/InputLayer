using System.Collections.Generic;

namespace InputLayer.Settings
{
    public class SettingsServiceOptions
    {
        public bool CreateBackup { get; set; } = true;

        public bool Indent { get; set; } = true;

        public string IndentChars { get; set; } = "    ";

        public Dictionary<string, string> NamespacePrefixMap { get; set; } = new Dictionary<string, string>();
    }
}