using System;
using System.Globalization;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class NullToBooleanConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is null;
    }
}