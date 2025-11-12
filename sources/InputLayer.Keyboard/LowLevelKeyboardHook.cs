using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using InputLayer.Keyboard.Native;

namespace InputLayer.Keyboard
{
    public sealed class LowLevelKeyboardHook : IDisposable
    {
        private readonly Dictionary<Keys, bool> _keyStates = new Dictionary<Keys, bool>();
        private readonly NativeMethods.LowLevelKeyboardProc _proc;
        private IntPtr _hookId = IntPtr.Zero;
        private bool _isDisposed;

        public LowLevelKeyboardHook()
        {
            _proc = this.HookCallback;
            this.InitializeKeyStates();
        }

        ~LowLevelKeyboardHook()
        {
            this.Dispose(false);
        }

        public event EventHandler<LowLevelKeyEventArgs> KeyDown;

        public event EventHandler<LowLevelKeyEventArgs> KeyUp;

        public bool IsHooked => _hookId != IntPtr.Zero;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (_hookId == IntPtr.Zero)
            {
                _hookId = this.SetHook(_proc);
            }
        }

        public void Stop()
        {
            if (_hookId != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    this.Stop();
                }

                _isDisposed = true;
            }
        }

        private Modifiers[] GetCurrentModifiers()
        {
            var modifiers = new List<Modifiers>();

            if (_keyStates.TryGetValue(Keys.LeftControl, out var leftControlValue) && leftControlValue)
            {
                modifiers.Add(Modifiers.LeftControl);
            }

            if (_keyStates.TryGetValue(Keys.RightControl, out var rightControlValue) && rightControlValue)
            {
                modifiers.Add(Modifiers.RightControl);
            }

            if (_keyStates.TryGetValue(Keys.LeftAlt, out var leftAltValue) && leftAltValue)
            {
                modifiers.Add(Modifiers.LeftAlt);
            }

            if (_keyStates.TryGetValue(Keys.RightAlt, out var rightAltValue) && rightAltValue)
            {
                modifiers.Add(Modifiers.RightAlt);
            }

            if (_keyStates.TryGetValue(Keys.LeftShift, out var leftShiftValue) && leftShiftValue)
            {
                modifiers.Add(Modifiers.LeftShift);
            }

            if (_keyStates.TryGetValue(Keys.RightShift, out var rightShiftValue) && rightShiftValue)
            {
                modifiers.Add(Modifiers.RightShift);
            }

            if (_keyStates.TryGetValue(Keys.LeftWin, out var leftWinValue) && leftWinValue)
            {
                modifiers.Add(Modifiers.LeftWin);
            }

            if (_keyStates.TryGetValue(Keys.RightWin, out var rightWinValue) && rightWinValue)
            {
                modifiers.Add(Modifiers.RightWin);
            }

            return modifiers.ToArray();
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = Marshal.PtrToStructure<NativeStructures.KBDLLHOOKSTRUCT>(lParam);
                var vkCode = hookStruct.vkCode;
                var key = (Keys)vkCode;

                if (IsModifierKey(key))
                {
                    if (wParam == (IntPtr)NativeConstants.WM_KEYDOWN || wParam == (IntPtr)NativeConstants.WM_SYSKEYDOWN)
                    {
                        _keyStates[key] = true;
                    }
                    else if (wParam == (IntPtr)NativeConstants.WM_KEYUP || wParam == (IntPtr)NativeConstants.WM_SYSKEYUP)
                    {
                        _keyStates[key] = false;
                    }
                }

                var currentModifiers = this.GetCurrentModifiers();
                var args = new LowLevelKeyEventArgs(key, currentModifiers);

                if (wParam == (IntPtr)NativeConstants.WM_KEYDOWN || wParam == (IntPtr)NativeConstants.WM_SYSKEYDOWN)
                {
                    this.KeyDown?.Invoke(this, args);
                }
                else if (wParam == (IntPtr)NativeConstants.WM_KEYUP || wParam == (IntPtr)NativeConstants.WM_SYSKEYUP)
                {
                    this.KeyUp?.Invoke(this, args);
                }

                if (args.Handled)
                {
                    return (IntPtr)1;
                }
            }

            return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        private void InitializeKeyStates()
        {
            var modifierKeys = new[]
            {
                Keys.LeftControl, Keys.RightControl,
                Keys.LeftAlt, Keys.RightAlt,
                Keys.LeftShift, Keys.RightShift,
                Keys.LeftWin, Keys.RightWin
            };

            foreach (var key in modifierKeys)
            {
                _keyStates[key] = false;
            }
        }

        private IntPtr SetHook(NativeMethods.LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            {
                using (var curModule = curProcess.MainModule)
                {
                    return NativeMethods.SetWindowsHookEx(NativeConstants.WH_KEYBOARD_LL, proc,
                                                          NativeMethods.GetModuleHandle(curModule?.ModuleName), 0);
                }
            }
        }

        #region Public Helper Methods

        public Modifiers[] GetPressedModifiers()
            => this.GetCurrentModifiers();

        public static bool IsProcessInForeground(int processId)
        {
            var foregroundWindow = NativeMethods.GetForegroundWindow();
            NativeMethods.GetWindowThreadProcessId(foregroundWindow, out var foregroundProcessId);
            return foregroundProcessId == processId;
        }

        public static bool IsModifierKey(Keys key)
            => key == Keys.LeftControl || key == Keys.RightControl ||
               key == Keys.LeftAlt || key == Keys.RightAlt ||
               key == Keys.LeftShift || key == Keys.RightShift ||
               key == Keys.LeftWin || key == Keys.RightWin;

        #endregion
    }

    public class LowLevelKeyEventArgs : EventArgs
    {
        public LowLevelKeyEventArgs(Keys key, Modifiers[] modifiers)
        {
            this.Key = key;
            this.Modifiers = modifiers;
            this.Handled = false;
        }

        public Keys Key { get; }

        public Modifiers[] Modifiers { get; }

        public bool Handled { get; set; }
    }
}