using System;
using System.Collections.Generic;
using System.Threading;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions.Settings;

namespace InputLayer.Common.Models.Actions
{
    public class SystemAction : ObservableObject, IActionWithParams
    {
        private SystemActionType _actionType;
        private bool _isOpenOptionalSettings;
        private object _settings;

        /// <inheritdoc/>
        public bool HasOptionalSettings
        {
            get
            {
                switch (this.ActionType)
                {
                    case SystemActionType.Pause:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public SystemActionType ActionType
        {
            get => _actionType;
            set => this.SetValue(ref _actionType, value);
        }

        /// <inheritdoc/>
        public bool IsOpenOptionalSettings
        {
            get => _isOpenOptionalSettings;
            set => this.SetValue(ref _isOpenOptionalSettings, value);
        }

        public object Settings
        {
            get => _settings ?? (_settings = this.GetDefaultSettings());
            set => this.SetValue(ref _settings, value);
        }

        /// <inheritdoc/>
        public void Execute(object obj = null)
        {
            switch (this.ActionType)
            {
                case SystemActionType.Pause:
                    var settings = SystemActionPauseSettings.GetOrDefault(this.Settings);
                    Thread.Sleep(settings.Timeout);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"System: {this.ActionType}";

        private object GetDefaultSettings()
        {
            switch (this.ActionType)
            {
                case SystemActionType.Pause:
                    return SystemActionPauseSettings.Default;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}