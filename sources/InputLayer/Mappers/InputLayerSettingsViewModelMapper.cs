using System;
using System.Collections.ObjectModel;
using System.Linq;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;
using InputLayer.Common.Models.Actions.Settings;
using InputLayer.Models;
using InputLayer.Settings.Mappers;
using InputLayer.Settings.Models;
using InputLayer.Settings.Models.Actions;
using InputLayer.Settings.Models.Actions.Settings;
using ControllerAction = InputLayer.Models.ControllerAction;

namespace InputLayer.Mappers
{
    public class InputLayerSettingsMapper : IViewModelMapper<InputLayerSettingsData, InputLayerSettings>
    {
        public static InputLayerSettingsMapper Default { get; } = new InputLayerSettingsMapper();

        /// <inheritdoc/>
        public InputLayerSettingsData FromViewModel(InputLayerSettings viewModel)
            => new InputLayerSettingsData
            {
                MainButton = viewModel.MainButton,
                DisplayMode = viewModel.DisplayMode,
                DesktopActions = viewModel.DesktopActions.Select(ControllerActionMapper.Default.FromViewModel).ToList(),
                FullScreenActions = viewModel.FullScreenActions.Select(ControllerActionMapper.Default.FromViewModel).ToList(),
                InGameActions = viewModel.InGameActions.Select(ControllerActionMapper.Default.FromViewModel).ToList()
            };

        /// <inheritdoc/>
        public InputLayerSettings ToViewModel(InputLayerSettingsData data)
            => new InputLayerSettings
            {
                MainButton = data.MainButton,
                DisplayMode = data.DisplayMode,
                DesktopActions = new ObservableCollection<ControllerAction>(data.DesktopActions.Select(ControllerActionMapper.Default.ToViewModel)),
                FullScreenActions = new ObservableCollection<ControllerAction>(data.FullScreenActions.Select(ControllerActionMapper.Default.ToViewModel)),
                InGameActions = new ObservableCollection<ControllerAction>(data.InGameActions.Select(ControllerActionMapper.Default.ToViewModel))
            };
    }

    public class ControllerActionMapper : IViewModelMapper<ControllerActionData, ControllerAction>
    {
        public static ControllerActionMapper Default { get; } = new ControllerActionMapper();

        /// <inheritdoc/>
        public ControllerActionData FromViewModel(ControllerAction viewModel)
            => new ControllerActionData
            {
                Button = viewModel.Button,
                Mode = viewModel.Mode,
                IsPredefined = viewModel.IsPredefined,
                Actions = viewModel.Actions.Select(ControllerActionItemMapper.Default.FromViewModel).ToList()
            };

        /// <inheritdoc/>
        public ControllerAction ToViewModel(ControllerActionData data)
            => new ControllerAction
            {
                Button = data.Button,
                Mode = data.Mode,
                IsPredefined = data.IsPredefined,
                Actions = new ObservableCollection<ControllerActionItem>(data.Actions.Select(ControllerActionItemMapper.Default.ToViewModel))
            };
    }

    public class ControllerActionItemMapper : IViewModelMapper<ControllerActionItemData, ControllerActionItem>
    {
        public static ControllerActionItemMapper Default { get; } = new ControllerActionItemMapper();

        /// <inheritdoc/>
        public ControllerActionItemData FromViewModel(ControllerActionItem viewModel)
            => new ControllerActionItemData
            {
                ActionType = viewModel.ActionType,
                Action = ActionMapper.Default.FromViewModel(viewModel.Action)
            };

        /// <inheritdoc/>
        public ControllerActionItem ToViewModel(ControllerActionItemData data)
            => new ControllerActionItem
            {
                ActionType = data.ActionType,
                Action = ActionMapper.Default.ToViewModel(data.Action)
            };
    }

    public class ActionMapper : IViewModelMapper<ActionDataBase, IAction>
    {
        public static ActionMapper Default { get; } = new ActionMapper();

        /// <inheritdoc/>
        public ActionDataBase FromViewModel(IAction viewModel)
        {
            switch (viewModel)
            {
                case CommandAction commandAction:
                    return CreateCommandActionData(commandAction);
                case ExecutableAction executableAction:
                    return CreateExecutableActionData(executableAction);
                case GameControllerAction gameControllerAction:
                    return CreateGameControllerActionData(gameControllerAction);
                case KeyboardAction keyboardAction:
                    return this.CreateKeyboardActionData(keyboardAction);
                case PlayniteAction playniteAction:
                    return this.CreatePlayniteActionData(playniteAction);
                case PowerShellCommandAction powerShellCommandAction:
                    return this.CreatePowerShellCommandActionData(powerShellCommandAction);
                case SystemAction systemAction:
                    return this.CreateSystemActionData(systemAction);
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel, null);
            }
        }

        /// <inheritdoc/>
        public IAction ToViewModel(ActionDataBase data)
        {
            switch (data)
            {
                case CommandActionData commandAction:
                    return this.CreateCommandAction(commandAction);
                case ExecutableActionData executableAction:
                    return this.CreateExecutableAction(executableAction);
                case GameControllerActionData gameControllerAction:
                    return this.CreateGameControllerAction(gameControllerAction);
                case KeyboardActionData keyboardAction:
                    return this.CreateKeyboardAction(keyboardAction);
                case PlayniteActionData playniteAction:
                    return this.CreatePlayniteAction(playniteAction);
                case PowerShellCommandActionData powerShellCommandAction:
                    return this.CreatePowerShellCommandAction(powerShellCommandAction);
                case SystemActionData systemAction:
                    return this.CreateSystemAction(systemAction);
                default:
                    throw new ArgumentOutOfRangeException(nameof(data), data, null);
            }
        }

        private static ActionDataBase CreateCommandActionData(CommandAction action)
            => new CommandActionData
            {
                Command = action.Command,
                IsHidden = action.IsHidden,
                WorkingDirectory = action.WorkingDirectory
            };

        private static ActionDataBase CreateExecutableActionData(ExecutableAction action)
            => new ExecutableActionData
            {
                FileName = action.FileName,
                IsHidden = action.IsHidden,
                Arguments = action.Arguments,
                WorkingDirectory = action.WorkingDirectory
            };

        private static ActionDataBase CreateGameControllerActionData(GameControllerAction action)
        {
            var gameControllerActionData = new GameControllerActionData
            {
                ActionType = action.ActionType
            };

            switch (action.ActionType)
            {
                case GameControllerActionType.Rumble when action.Settings is GameControllerActionRumbleSettings settings:
                    gameControllerActionData.Settings = new GameControllerActionRumbleSettingsData
                    {
                        DurationMs = settings.DurationMs,
                        Intensity = settings.Intensity
                    };
                    break;
            }

            return gameControllerActionData;
        }

        private IAction CreateCommandAction(CommandActionData data)
            => new CommandAction
            {
                Command = data.Command,
                IsHidden = data.IsHidden,
                WorkingDirectory = data.WorkingDirectory
            };

        private IAction CreateExecutableAction(ExecutableActionData data)
            => new ExecutableAction
            {
                FileName = data.FileName,
                IsHidden = data.IsHidden,
                Arguments = data.Arguments,
                WorkingDirectory = data.WorkingDirectory
            };

        private IAction CreateGameControllerAction(GameControllerActionData data)
        {
            var gameControllerAction = new GameControllerAction
            {
                ActionType = data.ActionType
            };

            switch (data.ActionType)
            {
                case GameControllerActionType.Rumble when data.Settings is GameControllerActionRumbleSettingsData settings:
                    gameControllerAction.Settings = new GameControllerActionRumbleSettings
                    {
                        DurationMs = settings.DurationMs,
                        Intensity = settings.Intensity
                    };
                    break;
            }

            return gameControllerAction;
        }

        private IAction CreateKeyboardAction(KeyboardActionData data)
            => new KeyboardAction
            {
                Key = data.Key,
                Modifiers = data.Modifiers
            };

        private ActionDataBase CreateKeyboardActionData(KeyboardAction action)
            => new KeyboardActionData
            {
                Key = action.Key,
                Modifiers = action.Modifiers
            };

        private IAction CreatePlayniteAction(PlayniteActionData data)
            => new PlayniteAction
            {
                ActionType = data.ActionType
            };

        private ActionDataBase CreatePlayniteActionData(PlayniteAction action)
            => new PlayniteActionData
            {
                ActionType = action.ActionType
            };

        private IAction CreatePowerShellCommandAction(PowerShellCommandActionData data)
            => new PowerShellCommandAction
            {
                Command = data.Command,
                IsHidden = data.IsHidden,
                WorkingDirectory = data.WorkingDirectory
            };

        private ActionDataBase CreatePowerShellCommandActionData(PowerShellCommandAction action)
            => new PowerShellCommandActionData
            {
                Command = action.Command,
                IsHidden = action.IsHidden,
                WorkingDirectory = action.WorkingDirectory
            };

        private IAction CreateSystemAction(SystemActionData data)
        {
            var systemActionData = new SystemAction
            {
                ActionType = data.ActionType
            };

            switch (data.ActionType)
            {
                case SystemActionType.Pause when data.Settings is SystemActionPauseSettingsData settings:
                    systemActionData.Settings = new SystemActionPauseSettings
                    {
                        Timeout = settings.Timeout
                    };
                    break;
            }

            return systemActionData;
        }

        private ActionDataBase CreateSystemActionData(SystemAction action)
        {
            var systemActionData = new SystemActionData
            {
                ActionType = action.ActionType
            };

            switch (action.ActionType)
            {
                case SystemActionType.Pause when action.Settings is SystemActionPauseSettings settings:
                    systemActionData.Settings = new SystemActionPauseSettingsData
                    {
                        Timeout = settings.Timeout
                    };
                    break;
            }

            return systemActionData;
        }
    }
}