// ReSharper disable All

namespace InputLayer.Agent
{
    public static partial class SDL
    {
        #region SDL Library Name

        private const string nativeLibName = "SDL2";

        #endregion

        #region SDL Init Flags

        public const uint SDL_INIT_JOYSTICK = 0x00000200;
        public const uint SDL_INIT_HAPTIC = 0x00001000;
        public const uint SDL_INIT_GAMECONTROLLER = 0x00002000;
        public const uint SDL_INIT_EVENTS = 0x00004000;

        #endregion

        #region SDL Hints

        public const string SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS = "SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS";
        public const string SDL_HINT_ACCELEROMETER_AS_JOYSTICK = "SDL_ACCELEROMETER_AS_JOYSTICK";
        public const string SDL_HINT_MAC_BACKGROUND_APP = "SDL_MAC_BACKGROUND_APP";
        public const string SDL_HINT_XINPUT_ENABLED = "SDL_XINPUT_ENABLED";
        public const string SDL_HINT_JOYSTICK_RAWINPUT = "SDL_JOYSTICK_RAWINPUT";

        #endregion
    }
}