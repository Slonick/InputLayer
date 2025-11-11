using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace InputLayer.AttachedProperties
{
    public static class ComboBoxRefreshBindingOnLoadBehavior
    {
        public static readonly DependencyProperty RefreshBindingOnLoadProperty =
            DependencyProperty.RegisterAttached("RefreshBindingOnLoad",
                                                typeof(bool),
                                                typeof(ComboBoxRefreshBindingOnLoadBehavior),
                                                new PropertyMetadata(false, OnRefreshBindingOnLoadChanged));

        public static bool GetRefreshBindingOnLoad(DependencyObject obj)
            => (bool)obj.GetValue(RefreshBindingOnLoadProperty);

        public static void SetRefreshBindingOnLoad(DependencyObject obj, bool value)
            => obj.SetValue(RefreshBindingOnLoadProperty, value);

        private static void OnComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                comboBox.Dispatcher.BeginInvoke(new Action(() => { RefreshComboBoxBinding(comboBox); }), DispatcherPriority.DataBind);
            }
        }

        private static void OnRefreshBindingOnLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox && (bool)e.NewValue)
            {
                comboBox.Loaded += OnComboBoxLoaded;
            }
            else if (d is ComboBox cb && !(bool)e.NewValue)
            {
                cb.Loaded -= OnComboBoxLoaded;
            }
        }

        private static void RefreshComboBoxBinding(ComboBox comboBox)
        {
            var bindingExpression = BindingOperations.GetMultiBindingExpression(comboBox, ItemsControl.ItemsSourceProperty);
            bindingExpression?.UpdateTarget();
        }
    }
}