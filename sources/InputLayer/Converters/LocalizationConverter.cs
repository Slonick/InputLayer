using System;
using System.Globalization;
using System.Windows;
using InputLayer.Converters.Base;
using Playnite.SDK;

namespace InputLayer.Converters
{
    public class LocalizationConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is object)
            {
                if (parameter is string format)
                {
                    var formatedKey = string.Format(format, value);
                    return ResourceProvider.GetString(formatedKey);
                }

                return ResourceProvider.GetString(value.ToString());
            }

            return DependencyProperty.UnsetValue;
        }
    }
}