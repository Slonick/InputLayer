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
            if (values.Length == 3 &&
                values[0] is ControllerInput mainButton &&
                values[1] is ControllerInput button &&
                values[2] is ControllerInputDisplayMode displayMode)
            {
                switch (button)
                {
                    case ControllerInput.None:
                        return null;
                    case ControllerInput.Main:
                        return ControllerButtonHelper.GetDisplayName(mainButton, displayMode);
                    case ControllerInput.LongPress:
                        return $"{ControllerButtonHelper.GetDisplayName(mainButton, displayMode)} {ResourceProvider.GetString("InputLayer.LongPress")}";
                    default:
                        return $"{ControllerButtonHelper.GetDisplayName(mainButton, displayMode)} + {ControllerButtonHelper.GetDisplayName(button, displayMode)}";
                }
            }

            return Binding.DoNothing;
        }
    }
}