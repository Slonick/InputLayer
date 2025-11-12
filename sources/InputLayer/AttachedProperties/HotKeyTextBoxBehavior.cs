using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using InputLayer.Common.Models.Actions;
using InputLayer.Keyboard;

namespace InputLayer.AttachedProperties
{
    public static class HotKeyTextBoxBehavior
    {
        private static readonly HashSet<TextBox> _activeTextBoxes = new HashSet<TextBox>();
        private static readonly int _currentProcessId = Process.GetCurrentProcess().Id;
        private static readonly LowLevelKeyboardHook _globalHook;
        private static TextBox _focusedTextBox;

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(HotKeyTextBoxBehavior),
                                                new PropertyMetadata(false, OnIsEnabledChanged));

        static HotKeyTextBoxBehavior()
        {
            // Initialize global hook
            _globalHook = new LowLevelKeyboardHook();
            _globalHook.KeyDown += OnGlobalKeyDown;
        }

        public static bool GetIsEnabled(DependencyObject obj)
            => (bool)obj.GetValue(IsEnabledProperty);

        public static void SetIsEnabled(DependencyObject obj, bool value)
            => obj.SetValue(IsEnabledProperty, value);

        private static void EnsureHookStarted()
        {
            // Start hook only if we have a focused textbox
            if (!_globalHook.IsHooked && _focusedTextBox != null)
            {
                _globalHook.Start();
            }
        }

        private static void OnGlobalKeyDown(object sender, LowLevelKeyEventArgs e)
        {
            // Only process if we have a focused textbox AND our window is active
            if (_focusedTextBox == null || !_activeTextBoxes.Contains(_focusedTextBox))
            {
                return;
            }

            // Check if our window is actually in focus
            if (!LowLevelKeyboardHook.IsProcessInForeground(_currentProcessId))
            {
                return;
            }

            var key = e.Key;

            // Don't process modifier keys alone
            if (LowLevelKeyboardHook.IsModifierKey(key))
            {
                e.Handled = true;
                return;
            }

            // Handle Escape to clear the hotkey
            if (key == Keys.Escape && _globalHook.GetPressedModifiers().Length == 0)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_focusedTextBox?.DataContext is KeyboardAction action)
                    {
                        action.Modifiers = Array.Empty<Modifiers>();
                        action.Key = Keys.None;
                        _focusedTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
                    }
                }));

                e.Handled = true;
                return;
            }

            // Get modifiers BEFORE dispatcher invoke
            var modifiers = _globalHook.GetPressedModifiers();

            if (key != Keys.None)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_focusedTextBox?.DataContext is KeyboardAction action)
                    {
                        action.Modifiers = modifiers;
                        action.Key = key;
                        _focusedTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
                    }
                }));

                e.Handled = true;
            }
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _focusedTextBox = textBox;
                textBox.SelectAll();

                // Start the hook when TextBox gets focus
                EnsureHookStarted();
            }
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    _activeTextBoxes.Add(textBox);
                    textBox.GotFocus += OnGotFocus;
                    textBox.LostFocus += OnLostFocus;
                    textBox.IsVisibleChanged += OnIsVisibleChanged;
                    textBox.Unloaded += OnUnloaded;

                    // Don't start hook here - only when TextBox gets focus
                }
                else
                {
                    _activeTextBoxes.Remove(textBox);
                    textBox.GotFocus -= OnGotFocus;
                    textBox.LostFocus -= OnLostFocus;
                    textBox.IsVisibleChanged -= OnIsVisibleChanged;
                    textBox.Unloaded -= OnUnloaded;

                    if (_focusedTextBox == textBox)
                    {
                        _focusedTextBox = null;

                        // Stop the hook if this was the focused TextBox
                        StopHookIfNotNeeded();
                    }
                }
            }
        }

        private static void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox && !(bool)e.NewValue && _focusedTextBox == textBox)
            {
                _focusedTextBox = null;

                // Stop the hook when focused TextBox becomes invisible
                StopHookIfNotNeeded();
            }
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && _focusedTextBox == textBox)
            {
                _focusedTextBox = null;

                // Stop the hook when TextBox loses focus
                StopHookIfNotNeeded();
            }
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _activeTextBoxes.Remove(textBox);

                if (_focusedTextBox == textBox)
                {
                    _focusedTextBox = null;

                    // Stop the hook when focused TextBox is unloaded
                    StopHookIfNotNeeded();
                }
            }
        }

        private static void StopHookIfNotNeeded()
        {
            // Stop hook if no textbox is focused
            if (_globalHook.IsHooked && _focusedTextBox == null)
            {
                _globalHook.Stop();
            }
        }
    }
}