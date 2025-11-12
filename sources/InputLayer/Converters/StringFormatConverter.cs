using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class StringFormatConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] is string format && !string.IsNullOrWhiteSpace(format))
            {
                return string.Format(format, values.Skip(1).ToArray());
            }

            return Binding.DoNothing;
        }
    }
}