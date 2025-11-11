using System.Collections.Generic;

namespace InputLayer.Common.Models.Actions.Settings
{
    public class SystemActionPauseSettings : ObservableObject
    {
        private int _timeout;

        public static SystemActionPauseSettings Default
            => new SystemActionPauseSettings
            {
                Timeout = 300
            };

        public int Timeout
        {
            get => _timeout;
            set => this.SetValue(ref _timeout, value);
        }

        public static SystemActionPauseSettings GetOrDefault(object obj)
        {
            if (obj is SystemActionPauseSettings settings)
            {
                return settings;
            }

            return Default;
        }
    }
}