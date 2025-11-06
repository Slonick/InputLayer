using System;
using System.Globalization;
using System.Windows;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class InvertBooleanConverter : MarkupValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBool)
            {
                return !valueAsBool;
            }

            return DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBool)
            {
                return !valueAsBool;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}