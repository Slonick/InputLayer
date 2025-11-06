using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class BooleanToGeometryConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is bool state &&
                values[1] is Geometry trueGeometry &&
                values[2] is Geometry falseGeometry)
            {
                return state ? trueGeometry : falseGeometry;
            }

            return Binding.DoNothing;
        }
    }
}