using System;
using System.Collections.Generic;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;

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
                this.HandleActionTypeChange(value);
            }
        }

        internal static ControllerActionItem Default()
            => new ControllerActionItem
            {
                ActionType = ActionType.Command,
                Action = new CommandAction()
            };

        private void HandleActionTypeChange(ActionType actionType)
        {
            Type type;
            switch (actionType)
            {
                case ActionType.Command:
                    type = typeof(CommandAction);
                    break;
                case ActionType.PowerShellCommand:
                    type = typeof(PowerShellCommandAction);
                    break;
                case ActionType.Executable:
                    type = typeof(ExecutableAction);
                    break;
                case ActionType.Keyboard:
                    type = typeof(KeyboardAction);
                    break;
                case ActionType.Playnite:
                    type = typeof(PlayniteAction);
                    break;
                case ActionType.GameController:
                    type = typeof(GameControllerAction);
                    break;
                case ActionType.System:
                    type = typeof(SystemAction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }

            if (type != this.Action.GetType())
            {
                this.Action = Activator.CreateInstance(type) as IAction;
            }
        }
    }
}