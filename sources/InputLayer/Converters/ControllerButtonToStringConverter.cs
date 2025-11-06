using System;
using System.Globalization;
using InputLayer.Common.Infrastructures;
using InputLayer.Converters.Base;
using InputLayer.Helpers;

namespace InputLayer.Converters
{
    public class ControllerButtonToStringConverter : MarkupValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ControllerInput button))
            {
                return "Unknown";
            }

            return ControllerButtonHelper.GetDisplayName(button);
        }
    }
}