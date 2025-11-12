using System;
using System.Globalization;
using System.Windows.Data;
using InputLayer.Common.Infrastructures;
using InputLayer.Converters.Base;
using InputLayer.Helpers;

namespace InputLayer.Converters
{
    public class ControllerButtonToStringConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is ControllerInput button &&
                values[1] is ControllerInputDisplayMode displayMode)
            {
                switch (button)
                {
                    case ControllerInput.None:
                        return null;
                    default:
                        return ControllerButtonHelper.GetDisplayName(button, displayMode);
                }
            }

            return Binding.DoNothing;
        }
    }
}