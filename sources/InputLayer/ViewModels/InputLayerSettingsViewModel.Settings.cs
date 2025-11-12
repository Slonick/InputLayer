using System.Collections.Generic;
using InputLayer.Models;
using Playnite.SDK;

namespace InputLayer.ViewModels
{
    public partial class InputLayerSettingsViewModel : ISettings
    {
        private InputLayerSettings _editingClone;
        private InputLayerSettings _settings;

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
            _editingClone = _settingsManager.GetClone(this.Settings);

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
            _settingsManager.SavePluginSettings(this.Settings);
            _controllerService.ButtonPressed -= this.OnButtonPressed;
        }

        /// <inheritdoc/>
        public bool VerifySettings(out List<string> errors)
        {
            this.Settings.Validate(out errors);
            return errors.Count == 0;
        }
    }
}