using System;
using System.Runtime.InteropServices;

// ReSharper disable All
namespace InputLayer.Agent
{
    public static partial class SDL
    {
        #region SDL_version.h

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetVersion(out SDL_version ver);

        #endregion

        #region SDL_events.h

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PollEvent(out SDL_Event _event);

        #endregion

        #region SDL_stdinc.h

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SDL_free(IntPtr memblock);

        #endregion

        #region SDL.h

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_Init(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();

        #endregion

        #region SDL_hints.h

        [DllImport(nativeLibName, EntryPoint = "SDL_SetHint", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe SDL_bool INTERNAL_SDL_SetHint(byte* name, byte* value);

        public static unsafe SDL_bool SDL_SetHint(string name, string value)
        {
            var utf8Name = Utf8EncodeHeap(name);
            var utf8Value = Utf8EncodeHeap(value);
            var result = INTERNAL_SDL_SetHint(utf8Name, utf8Value);
            Marshal.FreeHGlobal((IntPtr)utf8Value);
            Marshal.FreeHGlobal((IntPtr)utf8Name);
            return result;
        }

        #endregion

        #region SDL_error.h

        [DllImport(nativeLibName, EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetError();

        public static string SDL_GetError()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetError());
        }

        #endregion

        #region SDL_joystick.h

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_NumJoysticks();

        [DllImport(nativeLibName, EntryPoint = "SDL_JoystickName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_JoystickName(IntPtr joystick);

        public static string SDL_JoystickName(IntPtr joystick)
        {
            return UTF8_ToManaged(INTERNAL_SDL_JoystickName(joystick));
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickInstanceID(IntPtr joystick);

        #endregion

        #region SDL_gamecontroller.h

        [DllImport(nativeLibName, EntryPoint = "SDL_GameControllerAddMapping", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_SDL_GameControllerAddMapping(byte* mappingString);

        public static unsafe int SDL_GameControllerAddMapping(string mappingString)
        {
            var utf8Mapping = Utf8EncodeHeap(mappingString);
            var result = INTERNAL_SDL_GameControllerAddMapping(utf8Mapping);
            Marshal.FreeHGlobal((IntPtr)utf8Mapping);
            return result;
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsGameController(int joystick_index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerOpen(int joystick_index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GameControllerClose(IntPtr gamecontroller);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GameControllerUpdate();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern short SDL_GameControllerGetAxis(IntPtr gamecontroller, SDL_GameControllerAxis axis);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GameControllerRumble(IntPtr gamecontroller, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

        #endregion
    }
}