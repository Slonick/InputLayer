using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InputLayer.Common.Models.Actions;
using InputLayer.Keyboard;

namespace InputLayer.AttachedProperties
{
    public static class HotKeyTextBoxBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(HotKeyTextBoxBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static Keys ConvertWpfKeyToCustomKey(Key key)
        {
            // A-Z
            if (key >= Key.A && key <= Key.Z)
            {
                return (Keys)((ushort)Keys.A + (key - Key.A));
            }

            // 0-9
            if (key >= Key.D0 && key <= Key.D9)
            {
                return (Keys)((ushort)Keys.D0 + (key - Key.D0));
            }

            // F1-F24
            if (key >= Key.F1 && key <= Key.F24)
            {
                var fKeyIndex = key - Key.F1;
                return (Keys)((ushort)Keys.F1 + fKeyIndex);
            }

            // Numpad 0-9
            if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                return (Keys)((ushort)Keys.NumPad0 + (key - Key.NumPad0));
            }

            switch (key)
            {
                // Control keys
                case Key.Space:
                    return Keys.Space;
                case Key.Enter:
                    return Keys.Enter;
                case Key.Tab:
                    return Keys.Tab;
                case Key.Back:
                    return Keys.Backspace;
                case Key.Delete:
                    return Keys.Delete;
                case Key.Escape:
                    return Keys.Escape;
                case Key.Clear:
                    return Keys.Clear;

                // Navigation
                case Key.Left:
                    return Keys.Left;
                case Key.Up:
                    return Keys.Up;
                case Key.Right:
                    return Keys.Right;
                case Key.Down:
                    return Keys.Down;
                case Key.Insert:
                    return Keys.Insert;
                case Key.Home:
                    return Keys.Home;
                case Key.End:
                    return Keys.End;
                case Key.PageUp:
                    return Keys.PageUp;
                case Key.PageDown:
                    return Keys.PageDown;
                case Key.Select:
                    return Keys.Select;
                case Key.Execute:
                    return Keys.Execute;
                case Key.Help:
                    return Keys.Help;

                // Modifiers
                case Key.LeftShift:
                    return Keys.LeftShift;
                case Key.RightShift:
                    return Keys.RightShift;
                case Key.LeftCtrl:
                    return Keys.LeftControl;
                case Key.RightCtrl:
                    return Keys.RightControl;
                case Key.LeftAlt:
                    return Keys.LeftAlt;
                case Key.RightAlt:
                    return Keys.RightAlt;
                case Key.LWin:
                    return Keys.LeftWin;
                case Key.RWin:
                    return Keys.RightWin;

                // Lock keys
                case Key.CapsLock:
                    return Keys.CapsLock;
                case Key.NumLock:
                    return Keys.NumLock;
                case Key.Scroll:
                    return Keys.ScrollLock;

                // Numpad operations
                case Key.Add:
                    return Keys.Add;
                case Key.Subtract:
                    return Keys.Subtract;
                case Key.Multiply:
                    return Keys.Multiply;
                case Key.Divide:
                    return Keys.Divide;
                case Key.Decimal:
                    return Keys.Decimal;
                case Key.Separator:
                    return Keys.Separator;

                // Special keys
                case Key.Apps:
                    return Keys.Apps;
                case Key.PrintScreen:
                    return Keys.PrintScreen;
                case Key.Pause:
                    return Keys.Pause;
                case Key.Sleep:
                    return Keys.Sleep;
                case Key.Print:
                    return Keys.Print;

                // OEM Keys
                case Key.OemSemicolon:
                    return Keys.OemSemicolon;
                case Key.OemPlus:
                    return Keys.OemPlus;
                case Key.OemComma:
                    return Keys.OemComma;
                case Key.OemMinus:
                    return Keys.OemMinus;
                case Key.OemPeriod:
                    return Keys.OemPeriod;
                case Key.OemQuestion:
                    return Keys.OemQuestion;
                case Key.OemTilde:
                    return Keys.OemTilde;
                case Key.OemOpenBrackets:
                    return Keys.OemOpenBrackets;
                case Key.OemPipe:
                    return Keys.OemPipe;
                case Key.OemCloseBrackets:
                    return Keys.OemCloseBrackets;
                case Key.OemQuotes:
                    return Keys.OemQuotes;
                case Key.OemBackslash:
                    return Keys.OemBackslash;
                case Key.Oem8:
                    return Keys.Oem8;
                case Key.OemClear:
                    return Keys.OemClear;

                // Browser keys
                case Key.BrowserBack:
                    return Keys.BrowserBack;
                case Key.BrowserForward:
                    return Keys.BrowserForward;
                case Key.BrowserRefresh:
                    return Keys.BrowserRefresh;
                case Key.BrowserStop:
                    return Keys.BrowserStop;
                case Key.BrowserSearch:
                    return Keys.BrowserSearch;
                case Key.BrowserFavorites:
                    return Keys.BrowserFavorites;
                case Key.BrowserHome:
                    return Keys.BrowserHome;

                // Media keys
                case Key.VolumeMute:
                    return Keys.VolumeMute;
                case Key.VolumeDown:
                    return Keys.VolumeDown;
                case Key.VolumeUp:
                    return Keys.VolumeUp;
                case Key.MediaNextTrack:
                    return Keys.MediaNextTrack;
                case Key.MediaPreviousTrack:
                    return Keys.MediaPrevTrack;
                case Key.MediaStop:
                    return Keys.MediaStop;
                case Key.MediaPlayPause:
                    return Keys.MediaPlayPause;

                // Launch keys
                case Key.LaunchMail:
                    return Keys.LaunchMail;
                case Key.SelectMedia:
                    return Keys.LaunchMediaSelect;
                case Key.LaunchApplication1:
                    return Keys.LaunchApp1;
                case Key.LaunchApplication2:
                    return Keys.LaunchApp2;

                // IME keys
                case Key.KanaMode:
                    return Keys.Kana;
                case Key.JunjaMode:
                    return Keys.Junja;
                case Key.FinalMode:
                    return Keys.Final;
                case Key.KanjiMode:
                    return Keys.Kanji;
                case Key.ImeConvert:
                    return Keys.Convert;
                case Key.ImeNonConvert:
                    return Keys.NonConvert;
                case Key.ImeAccept:
                    return Keys.Accept;
                case Key.ImeModeChange:
                    return Keys.ModeChange;
                case Key.ImeProcessed:
                    return Keys.ProcessKey;

                // Additional special keys
                case Key.Cancel:
                    return Keys.Cancel;
                case Key.Attn:
                    return Keys.Attn;
                case Key.CrSel:
                    return Keys.Crsel;
                case Key.ExSel:
                    return Keys.Exsel;
                case Key.EraseEof:
                    return Keys.EraseEof;
                case Key.Play:
                    return Keys.Play;
                case Key.Zoom:
                    return Keys.Zoom;
                case Key.Pa1:
                    return Keys.Pa1;

                // System key
                case Key.System:
                    return Keys.None;

                default:
                    return Keys.None;
            }
        }

        private static Modifiers[] GetModifiers()
        {
            var modifiers = new List<Modifiers>();

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                modifiers.Add(Modifiers.LeftControl);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.RightCtrl))
            {
                modifiers.Add(Modifiers.RightControl);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftAlt))
            {
                modifiers.Add(Modifiers.LeftAlt);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.RightAlt))
            {
                modifiers.Add(Modifiers.RightAlt);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift))
            {
                modifiers.Add(Modifiers.LeftShift);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
            {
                modifiers.Add(Modifiers.RightShift);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.LWin))
            {
                modifiers.Add(Modifiers.LeftWin);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.RWin))
            {
                modifiers.Add(Modifiers.RightWin);
            }

            return modifiers.ToArray();
        }

        private static bool IsModifierKey(Key key) => key == Key.LeftCtrl || key == Key.RightCtrl ||
                                                      key == Key.LeftAlt || key == Key.RightAlt ||
                                                      key == Key.LeftShift || key == Key.RightShift ||
                                                      key == Key.LWin || key == Key.RWin;

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewKeyDown += OnPreviewKeyDown;
                    textBox.GotFocus += OnGotFocus;
                }
                else
                {
                    textBox.PreviewKeyDown -= OnPreviewKeyDown;
                    textBox.GotFocus -= OnGotFocus;
                }
            }
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext != null)
            {
                var key = e.Key == Key.System ? e.SystemKey : e.Key;

                if (key == Key.Tab || key == Key.Enter)
                {
                    return;
                }

                if (IsModifierKey(key))
                {
                    e.Handled = true;
                    return;
                }

                if (key == Key.Escape && GetModifiers().Length == 0)
                {
                    if (textBox.DataContext is KeyboardAction action)
                    {
                        action.Modifiers = Array.Empty<Modifiers>();
                        action.Key = Keys.None;
                        textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
                    }

                    e.Handled = true;
                    return;
                }

                var modifiers = GetModifiers();
                var customKey = ConvertWpfKeyToCustomKey(key);

                if (customKey != Keys.None)
                {
                    if (textBox.DataContext is KeyboardAction action)
                    {
                        action.Modifiers = modifiers;
                        action.Key = customKey;

                        textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
                    }
                }

                e.Handled = true;
            }
        }
    }
}