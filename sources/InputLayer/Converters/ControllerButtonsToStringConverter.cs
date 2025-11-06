using System;
using System.Globalization;
using System.Windows.Data;
using InputLayer.Common.Infrastructures;
using InputLayer.Converters.Base;
using InputLayer.Helpers;
using Playnite.SDK;

namespace InputLayer.Converters
{
    public class ControllerButtonsToStringConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is ControllerInput mainButton &&
                values[1] is ControllerInput button)
            {
                switch (button)
                {
                    case ControllerInput.None:
                        return null;
                    case ControllerInput.Main:
                        return ControllerButtonHelper.GetDisplayName(mainButton);
                    case ControllerInput.LongPress:
                        return $"{ControllerButtonHelper.GetDisplayName(mainButton)} {ResourceProvider.GetString("LOCInputLayerLongPress")}";
                    default:
                        return $"{ControllerButtonHelper.GetDisplayName(mainButton)} + {ControllerButtonHelper.GetDisplayName(button)}";
                }
            }

            return Binding.DoNothing;
        }
    }
}