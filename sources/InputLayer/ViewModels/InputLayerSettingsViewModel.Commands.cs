using System.Windows.Input;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;
using Playnite.SDK;
using ControllerAction = InputLayer.Models.ControllerAction;

namespace InputLayer.ViewModels
{
    public partial class InputLayerSettingsViewModel
    {
        private InputLayerSettingsViewModel()
        {
            this.AddControllerActionCommand = new RelayCommand(this.AddControllerAction);
            this.RemoveControllerActionCommand = new RelayCommand<ControllerAction>(this.RemoveControllerAction);
            this.OpenExecutableCommand = new RelayCommand<ExecutableAction>(this.OpenExecutable);
            this.OpenWorkingDirectoryCommand = new RelayCommand<ExecutableAction>(this.OpenWorkingDirectory);
            this.ClearWorkingDirectoryCommand = new RelayCommand<ExecutableAction>(this.ClearWorkingDirectory);
            this.ToggleVisibleActionCommand = new RelayCommand<IExecutableAction>(this.ToggleVisibleAction);
            this.ToggleOptionalSettingsVisibilityCommand = new RelayCommand<IActionWithParams>(this.ToggleOptionalSettingsVisibility, this.ToggleOptionalSettingsVisibilityCanExecute);
            this.SetControllerInputDisplayModeCommand = new RelayCommand<ControllerInputDisplayMode>(this.SetControllerInputDisplayMode);
        }

        public ICommand AddControllerActionCommand { get; }

        public ICommand ClearWorkingDirectoryCommand { get; }

        public ICommand OpenExecutableCommand { get; }

        public ICommand OpenWorkingDirectoryCommand { get; }

        public ICommand RemoveControllerActionCommand { get; }

        public ICommand SetControllerInputDisplayModeCommand { get; }

        public ICommand ToggleOptionalSettingsVisibilityCommand { get; }

        public ICommand ToggleVisibleActionCommand { get; }

        private void AddControllerAction()
            => this.SelectedControllerActions.Add(ControllerAction.Default());

        private void ClearWorkingDirectory(ExecutableAction obj)
            => obj.WorkingDirectory = string.Empty;

        private void OpenExecutable(ExecutableAction obj)
        {
            const string executableFilter = "Executable files (*.exe)|*.exe";
            var selectFile = API.Instance.Dialogs.SelectFile(executableFilter);
            obj.FileName = selectFile;
        }

        private void OpenWorkingDirectory(ExecutableAction obj)
        {
            var selectFolder = API.Instance.Dialogs.SelectFolder();
            obj.WorkingDirectory = selectFolder;
        }

        private void RemoveControllerAction(ControllerAction controllerAction)
            => this.SelectedControllerActions.Remove(controllerAction);

        private void SetControllerInputDisplayMode(ControllerInputDisplayMode displayMode)
            => this.Settings.DisplayMode = displayMode;

        private void ToggleOptionalSettingsVisibility(IActionWithParams action)
            => action.IsOpenOptionalSettings = !action.IsOpenOptionalSettings;

        private bool ToggleOptionalSettingsVisibilityCanExecute(IActionWithParams action)
            => action?.HasOptionalSettings == true;

        private void ToggleVisibleAction(IExecutableAction action)
            => action.IsHidden = !action.IsHidden;
    }
}