using System;
using System.Globalization;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class TextToBooleanConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrWhiteSpace(value?.ToString());
    }
}