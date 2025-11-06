using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class EnumToSourceConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type enumType)
            {
                return Enum.GetValues(enumType)
                           .Cast<object>()
                           .ToList();
            }

            return Binding.DoNothing;
        }
    }
}