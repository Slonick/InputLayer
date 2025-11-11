using System;
using System.Globalization;
using System.Windows;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class BooleanToResourceConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
            {
                return null;
            }

            var valueAsBool = values[0] as bool? ?? false;
            var trueGeometryKey = values[1] as string;
            var falseGeometryKey = values[2] as string;

            var resourceKey = valueAsBool ? trueGeometryKey : falseGeometryKey;

            if (resourceKey != null && Application.Current.Resources.Contains(resourceKey))
            {
                return Application.Current.Resources[resourceKey];
            }

            return null;
        }
    }
}