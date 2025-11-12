using System;
using System.Runtime.InteropServices;
using System.Threading;
using InputLayer.Keyboard.Native;

namespace InputLayer.Keyboard
{
    public static class KeyboardSimulator
    {
        public static void KeyDown(Keys key)
        {
            var input = new NativeStructures.INPUT
            {
                type = NativeConstants.INPUT_KEYBOARD,
                u = new NativeStructures.InputUnion
                {
                    ki = new NativeStructures.KEYBDINPUT
                    {
                        wVk = (ushort)key,
                        wScan = NativeMethods.MapVirtualKey((uint)key, 0),
                        dwFlags = IsExtendedKey(key) ? NativeConstants.KEYEVENTF_EXTENDEDKEY : 0,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput(1, new[] { input }, Marshal.SizeOf(typeof(NativeStructures.INPUT)));
        }

        public static void KeyPress(Keys key, int delayMs = 10)
        {
            KeyDown(key);
            Thread.Sleep(delayMs);
            KeyUp(key);
        }

        public static void KeyPress(Modifiers modifier, Keys key, int delayMs = 10)
        {
            ModifierDown(modifier);
            Thread.Sleep(5);
            KeyDown(key);
            Thread.Sleep(delayMs);
            KeyUp(key);
            Thread.Sleep(5);
            ModifierUp(modifier);
        }

        public static void KeyPress(Modifiers[] modifiers, Keys key, int delayMs = 10)
        {
            foreach (var mod in modifiers)
            {
                ModifierDown(mod);
                Thread.Sleep(5);
            }

            KeyDown(key);
            Thread.Sleep(delayMs);
            KeyUp(key);

            for (var i = modifiers.Length - 1; i >= 0; i--)
            {
                Thread.Sleep(5);
                ModifierUp(modifiers[i]);
            }
        }

        public static void KeyUp(Keys key)
        {
            var input = new NativeStructures.INPUT
            {
                type = NativeConstants.INPUT_KEYBOARD,
                u = new NativeStructures.InputUnion
                {
                    ki = new NativeStructures.KEYBDINPUT
                    {
                        wVk = (ushort)key,
                        wScan = NativeMethods.MapVirtualKey((uint)key, 0),
                        dwFlags = NativeConstants.KEYEVENTF_KEYUP | (IsExtendedKey(key) ? NativeConstants.KEYEVENTF_EXTENDEDKEY : 0),
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput(1, new[] { input }, Marshal.SizeOf(typeof(NativeStructures.INPUT)));
        }

        public static void ModifierDown(Modifiers modifier)
        {
            if (modifier == Modifiers.None)
            {
                return;
            }

            var input = new NativeStructures.INPUT
            {
                type = NativeConstants.INPUT_KEYBOARD,
                u = new NativeStructures.InputUnion
                {
                    ki = new NativeStructures.KEYBDINPUT
                    {
                        wVk = (ushort)modifier,
                        wScan = NativeMethods.MapVirtualKey((uint)modifier, 0),
                        dwFlags = IsExtendedModifier(modifier) ? NativeConstants.KEYEVENTF_EXTENDEDKEY : 0,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput(1, new[] { input }, Marshal.SizeOf(typeof(NativeStructures.INPUT)));
        }

        public static void ModifierUp(Modifiers modifier)
        {
            if (modifier == Modifiers.None)
            {
                return;
            }

            var input = new NativeStructures.INPUT
            {
                type = NativeConstants.INPUT_KEYBOARD,
                u = new NativeStructures.InputUnion
                {
                    ki = new NativeStructures.KEYBDINPUT
                    {
                        wVk = (ushort)modifier,
                        wScan = NativeMethods.MapVirtualKey((uint)modifier, 0),
                        dwFlags = NativeConstants.KEYEVENTF_KEYUP | (IsExtendedModifier(modifier) ? NativeConstants.KEYEVENTF_EXTENDEDKEY : 0),
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput(1, new[] { input }, Marshal.SizeOf(typeof(NativeStructures.INPUT)));
        }

        public static void TypeText(string text, int delayBetweenKeys = 10)
        {
            foreach (var c in text)
            {
                var key = CharToKey(c, out var needShift);

                if (key == Keys.None)
                {
                    continue;
                }

                if (needShift)
                {
                    KeyPress(Modifiers.LeftShift, key, delayBetweenKeys);
                }
                else
                {
                    KeyPress(key, delayBetweenKeys);
                }

                Thread.Sleep(delayBetweenKeys);
            }
        }

        private static Keys CharToKey(char c, out bool needShift)
        {
            needShift = false;

            if (c >= 'a' && c <= 'z')
            {
                return (Keys)((int)Keys.A + (c - 'a'));
            }

            if (c >= 'A' && c <= 'Z')
            {
                needShift = true;
                return (Keys)((int)Keys.A + (c - 'A'));
            }

            if (c >= '0' && c <= '9')
            {
                return (Keys)((int)Keys.D0 + (c - '0'));
            }

            switch (c)
            {
                case ' ':
                    return Keys.Space;
                case '\t':
                    return Keys.Tab;
                case '\r':
                case '\n':
                    return Keys.Enter;

                case '!':
                    needShift = true;
                    return Keys.D1;
                case '@':
                    needShift = true;
                    return Keys.D2;
                case '#':
                    needShift = true;
                    return Keys.D3;
                case '$':
                    needShift = true;
                    return Keys.D4;
                case '%':
                    needShift = true;
                    return Keys.D5;
                case '^':
                    needShift = true;
                    return Keys.D6;
                case '&':
                    needShift = true;
                    return Keys.D7;
                case '*':
                    needShift = true;
                    return Keys.D8;
                case '(':
                    needShift = true;
                    return Keys.D9;
                case ')':
                    needShift = true;
                    return Keys.D0;

                default:
                    return Keys.None;
            }
        }

        private static bool IsExtendedKey(Keys key)
            => key == Keys.RightAlt ||
               key == Keys.RightControl ||
               key == Keys.Insert ||
               key == Keys.Delete ||
               key == Keys.Home ||
               key == Keys.End ||
               key == Keys.PageUp ||
               key == Keys.PageDown ||
               key == Keys.Left ||
               key == Keys.Up ||
               key == Keys.Right ||
               key == Keys.Down ||
               key == Keys.NumLock ||
               key == Keys.PrintScreen ||
               key == Keys.Divide;

        private static bool IsExtendedModifier(Modifiers modifier)
            => modifier == Modifiers.RightShift ||
               modifier == Modifiers.RightControl ||
               modifier == Modifiers.RightAlt;
    }
}