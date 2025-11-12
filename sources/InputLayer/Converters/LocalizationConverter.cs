using System;
using System.Globalization;
using System.Windows;
using InputLayer.Converters.Base;
using Playnite.SDK;
using ILogger = InputLayer.Common.Logging.ILogger;
using LogManager = InputLayer.Common.Logging.LogManager;

namespace InputLayer.Converters
{
    public class LocalizationConverter : MarkupValueConverter
    {
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is object)
            {
                var key = value.ToString();
                if (parameter is string format)
                {
                    key = string.Format(format, value);
                }

                var resource = ResourceProvider.GetString(key);
                if (resource.StartsWith("<!") && resource.EndsWith("!>"))
                {
                    _logger.Warn($"Localization key '{key}' not found.");
                }

                return resource;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}