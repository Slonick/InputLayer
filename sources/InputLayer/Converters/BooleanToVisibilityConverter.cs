using System;
using System.Globalization;
using System.Windows;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class BooleanToVisibilityConverter : MarkupValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBool)
            {
                return valueAsBool ? Visibility.Visible : Visibility.Collapsed;
            }

            return DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}