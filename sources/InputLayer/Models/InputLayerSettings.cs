using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;
using InputLayer.Infrastructures;
using InputLayer.Keyboard;
using Playnite.SDK;

namespace InputLayer.Models
{
    public class InputLayerSettings : ObservableObject
    {
        private ControllerInput _mainButton;

        public static InputLayerSettings Default { get; } = new InputLayerSettings
        {
            MainButton = ControllerInput.Guide,
            DesktopActions = new ObservableCollection<ControllerAction>
            {
                new ControllerAction
                {
                    IsPredefined = true,
                    Button = ControllerInput.Main,
                    Mode = ControllerButtonMode.Single,
                    Actions = new ObservableCollection<ControllerActionItem>
                    {
                        new ControllerActionItem
                        {
                            ActionType = ActionType.Playnite,
                            Action = new PlayniteAction
                            {
                                ActionType = PlayniteActionType.ToggleFullscreen,
                            }
                        }
                    }
                }
            },
            FullScreenActions = new ObservableCollection<ControllerAction>(),
            InGameActions = new ObservableCollection<ControllerAction>()
        };

        public ObservableCollection<ControllerAction> DesktopActions { get; set; }

        public ObservableCollection<ControllerAction> FullScreenActions { get; set; }

        public ObservableCollection<ControllerAction> InGameActions { get; set; }

        public ControllerInput MainButton
        {
            get => _mainButton;
            set => this.SetValue(ref _mainButton, value);
        }

        public void Validate(out List<string> errors)
        {
            errors = new List<string>();

            var controllerActions = this.DesktopActions.Concat(this.FullScreenActions).Concat(this.InGameActions);
            foreach (var controllerAction in controllerActions)
            {
                if (controllerAction.Button == ControllerInput.None)
                {
                    errors.Add(ResourceProvider.GetString("LOCInputLayerNoButtonSelectedError"));
                    return;
                }

                foreach (var action in controllerAction.Actions)
                {
                    switch (action.Action)
                    {
                        case CommandAction commandAction:
                            if (string.IsNullOrWhiteSpace(commandAction.Command))
                            {
                                errors.Add(ResourceProvider.GetString("LOCInputLayerEmptyCommandError"));
                                return;
                            }

                            break;
                        case ExecutableAction executableAction:
                            if (string.IsNullOrWhiteSpace(executableAction.FileName))
                            {
                                errors.Add(ResourceProvider.GetString("LOCInputLayerEmptyExecutableFilepathError"));
                                return;
                            }

                            if (!File.Exists(executableAction.FileName))
                            {
                                errors.Add(ResourceProvider.GetString("LOCInputLayerEmptyExecutableFileNotExistError"));
                                return;
                            }

                            break;
                        case KeyboardAction keyboardAction:
                            if (keyboardAction.Key == Keys.None)
                            {
                                errors.Add(ResourceProvider.GetString("LOCInputLayerEmptyHotkeyError"));
                                return;
                            }

                            break;
                    }
                }
            }
        }
    }
}