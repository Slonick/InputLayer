using System;
using System.Linq;
using System.Windows;
using InputLayer.Common.Logging;

namespace InputLayer.Controls
{
    public static class LocalizationLoader
    {
        private const string FallbackLanguage = "en_US";
        private const string LocalizationPattern = "/Resources/Localization/";
        private const string PluginName = nameof(InputLayer);

        private static readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        public static void LoadLanguage(string lang)
        {
            try
            {
                var resourcePath = $"/{PluginName};component{LocalizationPattern}{lang}.xaml";
                var uri = new Uri(resourcePath, UriKind.Relative);
                var resource = (ResourceDictionary)Application.LoadComponent(uri);

                var pluginResources = Application.Current.Resources.MergedDictionaries
                                                 .Where(d => d.Source?.OriginalString.IndexOf(PluginName, StringComparison.OrdinalIgnoreCase) >= 0);

                var oldResource = pluginResources
                    .FirstOrDefault(d => d.Source?.OriginalString.Contains(LocalizationPattern) ?? false);

                if (oldResource != null)
                {
                    var index = Application.Current.Resources.MergedDictionaries.IndexOf(oldResource);
                    Application.Current.Resources.MergedDictionaries[index] = resource;
                    _logger.Info($"Replaced localization resource with: {lang}");
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(resource);
                    _logger.Info($"Added localization resource: {lang}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to load localization '{lang}': {ex.Message}");

                if (lang != FallbackLanguage)
                {
                    _logger.Info($"Falling back to {FallbackLanguage}");
                    LoadLanguage(FallbackLanguage);
                }
            }
        }
    }
}