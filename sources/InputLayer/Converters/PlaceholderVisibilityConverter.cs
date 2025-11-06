using System;
using System.Globalization;
using System.Windows;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class PlaceholderVisibilityConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (values.Length)
            {
                case 1 when values[0] is string text:
                    return string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;
                case 2 when values[0] is string text && values[1] is bool isFocused:
                    return string.IsNullOrEmpty(text) && !isFocused ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return Visibility.Collapsed;
            }
        }
    }
}