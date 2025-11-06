using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;
using InputLayer.Common.Services;
using InputLayer.Infrastructures;
using InputLayer.Models;
using InputLayer.Services;
using InputLayer.Utils;
using Playnite.SDK;
using ControllerAction = InputLayer.Models.ControllerAction;

namespace InputLayer.ViewModels
{
    public class InputLayerSettingsViewModel : ObservableObject, ISettings
    {
        private readonly IControllerService _controllerService;
        private readonly SettingsService<InputLayerSettings> _settingsService;
        private ActionTab _actionTab = ActionTab.Desktop;
        private InputLayerSettings _editingClone;
        private InputLayerSettings _settings;

        public InputLayerSettingsViewModel(SettingsService<InputLayerSettings> settingsService, InputLayerSettings inputLayerSettings, IControllerService controllerService)
        {
            _settingsService = settingsService;
            _settings = inputLayerSettings;
            _controllerService = controllerService;

            this.AddControllerActionCommand = new RelayCommand(this.AddControllerAction);
            this.RemoveControllerActionCommand = new RelayCommand<ControllerAction>(this.RemoveControllerAction);
            this.OpenExecutableCommand = new RelayCommand<ExecutableAction>(this.OpenExecutable);
            this.ToggleVisibleActionCommand = new RelayCommand<IExecutableAction>(this.ToggleVisibleAction);
        }

        public ICommand AddControllerActionCommand { get; }
        public ObservableCollection<ControllerInput> ButtonLogs { get; } = new ObservableCollection<ControllerInput>();

        public ICommand OpenExecutableCommand { get; }

        public ICommand RemoveControllerActionCommand { get; }

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

        public ICommand ToggleVisibleActionCommand { get; }

        public ActionTab ActionTab
        {
            get => _actionTab;
            set
            {
                this.SetValue(ref _actionTab, value);
                this.OnPropertyChanged(nameof(this.SelectedControllerActions));
            }
        }

        public bool IsEditing { get; private set; }

        public InputLayerSettings Settings
        {
            get => _settings;
            private set => this.SetValue(ref _settings, value);
        }

        /// <inheritdoc/>
        public void BeginEdit()
        {
            this.ButtonLogs.Clear();

            this.IsEditing = true;
            _editingClone = _settingsService.GetClone(this.Settings);

            _controllerService.ButtonPressed += this.OnButtonPressed;
        }

        /// <inheritdoc/>
        public void CancelEdit()
        {
            this.IsEditing = false;
            this.Settings = _editingClone;

            _controllerService.ButtonPressed -= this.OnButtonPressed;
        }

        /// <inheritdoc/>
        public void EndEdit()
        {
            this.IsEditing = false;
            _settingsService.SavePluginSettings(this.Settings);
            _controllerService.ButtonPressed -= this.OnButtonPressed;
        }

        /// <inheritdoc/>
        public bool VerifySettings(out List<string> errors)
        {
            this.Settings.Validate(out errors);
            return errors.Count == 0;
        }

        private void AddControllerAction()
        {
            this.SelectedControllerActions.Add(ControllerAction.Default());
        }

        private void OnButtonPressed(ControllerInput button)
        {
            UIDispatcher.Invoke(() => this.ButtonLogs.Insert(0, button));
        }

        private void OpenExecutable(ExecutableAction obj)
        {
            const string executableFilter = "Executable files (*.exe)|*.exe";
            var selectFile = API.Instance.Dialogs.SelectFile(executableFilter);
            obj.FileName = selectFile;
        }

        private void RemoveControllerAction(ControllerAction controllerAction)
        {
            this.SelectedControllerActions.Remove(controllerAction);
        }

        private void ToggleVisibleAction(IExecutableAction action)
        {
            action.IsHidden = !action.IsHidden;
        }
    }
}