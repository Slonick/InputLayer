using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using InputLayer.Common.Infrastructures;
using InputLayer.Converters.Base;
using InputLayer.Helpers;

namespace InputLayer.Converters
{
    public class AvailableControllerButtonsConverter : MarkupMultiValueConverter
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                !(values[0] is ICollection<ControllerInput> buttons) ||
                !(values[1] is ItemsControl itemsControl) ||
                !(values[2] is ComboBox source))
            {
                return Binding.DoNothing;
            }

            var selectedItems = new List<ControllerInput>();

            foreach (var item in itemsControl.Items)
            {
                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    var comboBox = UIHelper.FindVisualChild<ComboBox>(container);
                    if ((comboBox?.SelectedItem ?? comboBox?.SelectedIndex) is ControllerInput selectedItem && comboBox != source)
                    {
                        selectedItems.Add(selectedItem);
                    }
                }
            }

            var availableButtons = buttons.Except(selectedItems).ToArray();
            if ((source.SelectedItem ?? source.SelectedValue) is ControllerInput sourceButton &&
                sourceButton != ControllerInput.None &&
                !availableButtons.Contains(sourceButton))
            {
                return availableButtons.Concat(new[] { sourceButton });
            }

            return availableButtons;
        }
    }
}