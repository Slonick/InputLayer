using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using InputLayer.Converters.Base;

namespace InputLayer.Converters
{
    public class PathToFileNameConverter : MarkupValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path)
            {
                return Path.GetFileName(path);
            }

            return Binding.DoNothing;
        }
    }
}