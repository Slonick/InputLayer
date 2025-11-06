using System;
using System.Globalization;
using System.Windows;
using InputLayer.Common.Infrastructures;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class ControllerButtonToVisibilityInvertedConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is ControllerInput button && button == ControllerInput.None
                ? Visibility.Visible
                : Visibility.Collapsed;
    }
}