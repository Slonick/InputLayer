using System;
using System.Windows;
using InputLayer.Common.Logging;

namespace InputLayer.Controls
{
    public static class ResourcesLoader
    {
        private const string PluginName = nameof(InputLayer);
        private static readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        public static void Load()
        {
            try
            {
                _logger.Info("Loading application resources...");

                var uri = new Uri($"/{PluginName};component/Resources/Resources.xaml", UriKind.Relative);
                var resourceDict = (ResourceDictionary)Application.LoadComponent(uri);
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);

                _logger.Info("Application resources loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to load application resources");
                throw;
            }
        }
    }
}