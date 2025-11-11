using System.Collections.Generic;

namespace InputLayer.Common.Models.Actions.Settings
{
    public class GameControllerActionRumbleSettings : ObservableObject
    {
        private int _durationMs;
        private float _intensity;

        public static GameControllerActionRumbleSettings Default
            => new GameControllerActionRumbleSettings
            {
                DurationMs = 200,
                Intensity = 0.5f
            };

        public int DurationMs
        {
            get => _durationMs;
            set => this.SetValue(ref _durationMs, value);
        }

        public float Intensity
        {
            get => _intensity;
            set => this.SetValue(ref _intensity, value);
        }

        public static GameControllerActionRumbleSettings GetOrDefault(object obj)
        {
            if (obj is GameControllerActionRumbleSettings settings)
            {
                return settings;
            }

            return Default;
        }

        public override string ToString()
            => $"Duration: {_durationMs}, Intensity: {_intensity}";
    }
}