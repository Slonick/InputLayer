using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Services;
using InputLayer.Infrastructures;
using InputLayer.Models;
using InputLayer.Services;
using Playnite.SDK;
using ControllerAction = InputLayer.Models.ControllerAction;
using ILogger = InputLayer.Common.Logging.ILogger;
using LogManager = InputLayer.Common.Logging.LogManager;

namespace InputLayer.ViewModels
{
    public partial class InputLayerSettingsViewModel : ObservableObject
    {
        private readonly IControllerService _controllerService;
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private readonly SettingsManager _settingsManager;
        private ActionTab _actionTab = ActionTab.Desktop;

        public InputLayerSettingsViewModel(
            SettingsManager settingsManager,
            InputLayerSettings inputLayerSettings,
            IControllerService controllerService) : this()
        {
            _settingsManager = settingsManager;
            _settings = inputLayerSettings;
            _controllerService = controllerService;
        }

        public ObservableCollection<ControllerInput> ButtonLogs { get; } = new ObservableCollection<ControllerInput>();

        public ObservableCollection<ControllerAction> SelectedControllerActions
        {
            get
            {
                switch (_actionTab)
                {
                    case ActionTab.Desktop:
                        return _settings.DesktopActions;
                    case ActionTab.Fullscreen:
                        return _settings.FullScreenActions;
                    case ActionTab.InGame:
                        return _settings.InGameActions;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ActionTab ActionTab
        {
            get => _actionTab;
            set
            {
                this.SetValue(ref _actionTab, value);
                this.OnPropertyChanged(nameof(this.SelectedControllerActions));
            }
        }

        private void OnButtonPressed(ControllerInput button)
        {
            _logger.Debug($"Button pressed: {button}");
            API.Instance.MainView.UIDispatcher.Invoke(() => this.ButtonLogs.Insert(0, button));
        }
    }
}