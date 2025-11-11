using System;
using System.Globalization;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class EqualToBooleanConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => Equals(value, parameter);
    }
}