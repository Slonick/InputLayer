using System;
using System.Collections.Generic;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions.Settings;
using InputLayer.Common.Services;

namespace InputLayer.Common.Models.Actions
{
    [Obsolete("Use GameControllerAction instead")]
    public class ControllerAction : GameControllerAction { }

    public class GameControllerAction : ObservableObject, IActionWithParams
    {
        private GameControllerActionType _actionType;
        private bool _isOpenOptionalSettings;
        private object _settings;

        /// <inheritdoc/>
        public bool HasOptionalSettings
        {
            get
            {
                switch (_actionType)
                {
                    case GameControllerActionType.Rumble:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public GameControllerActionType ActionType
        {
            get => _actionType;
            set
            {
                if (_actionType == value)
                {
                    return;
                }

                this.SetValue(ref _actionType, value);
                this.OnPropertyChanged(nameof(this.HasOptionalSettings));
                this.Settings = this.GetDefaultSettings();
            }
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
        public void Execute(object obj)
        {
            if (!(obj is IControllerService controllerService))
            {
                throw new ArgumentException($"Object must be of type {nameof(IControllerService)}", nameof(obj));
            }

            switch (this.ActionType)
            {
                case GameControllerActionType.Rumble:
                    var settings = GameControllerActionRumbleSettings.GetOrDefault(this.Settings);
                    controllerService.Rumble(settings.DurationMs, settings.Intensity);
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"Controller: {this.ActionType}";

        private object GetDefaultSettings()
        {
            switch (this.ActionType)
            {
                case GameControllerActionType.Rumble:
                    return GameControllerActionRumbleSettings.Default;
                default:
                    return null;
            }
        }
    }
}