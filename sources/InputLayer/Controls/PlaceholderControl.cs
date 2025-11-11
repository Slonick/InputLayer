using System.Windows;
using System.Windows.Controls;

namespace InputLayer.Controls
{
    public class PlaceholderControl : ContentControl
    {
        public static readonly DependencyProperty IsContentFocusedProperty =
            DependencyProperty.Register(nameof(IsContentFocused),
                                        typeof(bool),
                                        typeof(PlaceholderControl),
                                        new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsEmptyProperty =
            DependencyProperty.Register(nameof(IsEmpty),
                                        typeof(bool),
                                        typeof(PlaceholderControl),
                                        new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder),
                                        typeof(string),
                                        typeof(PlaceholderControl),
                                        new FrameworkPropertyMetadata(string.Empty));

        static PlaceholderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderControl),
                                                     new FrameworkPropertyMetadata(typeof(PlaceholderControl)));
        }

        public bool IsContentFocused
        {
            get => (bool)this.GetValue(IsContentFocusedProperty);
            set => this.SetValue(IsContentFocusedProperty, value);
        }

        public bool IsEmpty
        {
            get => (bool)this.GetValue(IsEmptyProperty);
            set => this.SetValue(IsEmptyProperty, value);
        }

        public string Placeholder
        {
            get => (string)this.GetValue(PlaceholderProperty);
            set => this.SetValue(PlaceholderProperty, value);
        }
    }
}