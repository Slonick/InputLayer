using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using InputLayer.Converters.Base;
using InputLayer.Keyboard;

namespace InputLayer.Converters
{
    public class KeyboardActionToTextConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is Keys key)
            {
                if (!(values[1] is Modifiers[] modificators))
                {
                    modificators = Array.Empty<Modifiers>();
                }

                if (modificators.Length == 0 && key == Keys.None)
                {
                    return null;
                }

                if (modificators.Length == 0)
                {
                    return key.ToString();
                }

                var modifiersList = new List<string>();

                if (modificators.Contains(Modifiers.LeftControl) || modificators.Contains(Modifiers.RightControl))
                {
                    modifiersList.Add("Ctrl");
                }

                if (modificators.Contains(Modifiers.LeftAlt) || modificators.Contains(Modifiers.RightAlt))
                {
                    modifiersList.Add("Alt");
                }

                if (modificators.Contains(Modifiers.LeftShift) || modificators.Contains(Modifiers.RightShift))
                {
                    modifiersList.Add("Shift");
                }

                if (modificators.Contains(Modifiers.LeftWin) || modificators.Contains(Modifiers.RightWin))
                {
                    modifiersList.Add("Win");
                }

                var modifiersText = string.Join(" + ", modifiersList);

                if (key != Keys.None)
                {
                    return $"{modifiersText} + {key}";
                }

                return modifiersText;
            }

            return Binding.DoNothing;
        }
    }
}