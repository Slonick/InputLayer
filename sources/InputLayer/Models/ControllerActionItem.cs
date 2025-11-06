using System;
using System.Collections.Generic;
using InputLayer.Common.Models.Actions;
using InputLayer.Infrastructures;

namespace InputLayer.Models
{
    public class ControllerActionItem : ObservableObject
    {
        private IAction _action = new CommandAction();
        private ActionType _actionType = ActionType.Command;

        public IAction Action
        {
            get => _action;
            set => this.SetValue(ref _action, value);
        }

        public ActionType ActionType
        {
            get => _actionType;
            set
            {
                if (_actionType == value)
                {
                    return;
                }

                this.SetValue(ref _actionType, value);

                switch (value)
                {
                    case ActionType.Command:
                        this.Action = this.Action is CommandAction ? this.Action : new CommandAction();
                        break;
                    case ActionType.PowerShellCommand:
                        this.Action = this.Action is PowerShellCommandAction ? this.Action : new PowerShellCommandAction();
                        break;
                    case ActionType.Executable:
                        this.Action = this.Action is ExecutableAction ? this.Action : new ExecutableAction();
                        break;
                    case ActionType.Keyboard:
                        this.Action = this.Action is KeyboardAction ? this.Action : new KeyboardAction();
                        break;
                    case ActionType.Playnite:
                        this.Action = this.Action is PlayniteAction ? this.Action : new PlayniteAction();
                        break;
                    case ActionType.Controller:
                        this.Action = this.Action is Common.Models.Actions.ControllerAction ? this.Action : new Common.Models.Actions.ControllerAction();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        internal static ControllerActionItem Default() => new ControllerActionItem
        {
            ActionType = ActionType.Command,
            Action = new CommandAction()
        };
    }
}