using System;
using System.IO;
using InputLayer.Common.Logging;
using InputLayer.Common.Utils;
using Playnite.SDK.Plugins;

namespace InputLayer.Services
{
    public class SettingsService<TSettings>
    {
        private readonly string _configFilepath;
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        public SettingsService(Plugin plugin)
        {
            _configFilepath = Path.Combine(plugin.GetPluginUserDataPath(), "config.json");
        }

        public TSettings GetClone(TSettings settings)
        {
            var source = Serializer.Serialize(settings);
            var clone = Serializer.Deserialize<TSettings>(source);
            return clone;
        }

        public TSettings LoadPluginSettings()
        {
            if (File.Exists(_configFilepath))
            {
                try
                {
                    var fileContent = File.ReadAllText(_configFilepath);
                    return Serializer.Deserialize<TSettings>(fileContent);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Failed to load plugin settings");
                }
            }

            return default;
        }

        public void SavePluginSettings(TSettings settings)
        {
            var jsonContent = Serializer.Serialize(settings);
            File.WriteAllText(_configFilepath, jsonContent);
        }
    }
}