using System;
using System.IO;
using InputLayer.Mappers;
using InputLayer.Models;
using InputLayer.Settings;
using InputLayer.Settings.Models;
using Playnite.SDK.Plugins;
using ILogger = InputLayer.Common.Logging.ILogger;
using LogManager = InputLayer.Common.Logging.LogManager;

namespace InputLayer.Services
{
    public class SettingsManager
    {
        private readonly string _configFilepath;
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private readonly SettingsService<InputLayerSettingsData, InputLayerSettings> _settingsService;

        public SettingsManager(Plugin plugin)
        {
            _configFilepath = Path.Combine(plugin.GetPluginUserDataPath(), "config.xml");

            var options = new SettingsServiceOptions
            {
                #if !DEBUG
                Indent = false,
                #endif
            };

            _settingsService = new SettingsService<InputLayerSettingsData, InputLayerSettings>(InputLayerSettingsMapper.Default, options);
        }

        public InputLayerSettings GetClone(InputLayerSettings settings)
            => _settingsService.Clone(settings);

        public InputLayerSettings LoadPluginSettings()
        {
            if (File.Exists(_configFilepath))
            {
                try
                {
                    return _settingsService.Load(_configFilepath);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Failed to load plugin settings");
                }
            }

            return null;
        }

        public void SavePluginSettings(InputLayerSettings settings)
            => _settingsService.Save(settings, _configFilepath);
    }
}