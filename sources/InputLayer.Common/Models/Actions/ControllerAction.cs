using System;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Services;

namespace InputLayer.Common.Models.Actions
{
    public class ControllerAction : IAction
    {
        public ControllerActionType ActionType { get; set; }

        /// <inheritdoc/>
        public void Execute(object obj)
        {
            if (!(obj is IControllerService controllerService))
            {
                throw new ArgumentException("Object must be of type IControllerService", nameof(obj));
            }

            switch (this.ActionType)
            {
                case ControllerActionType.Rumble:
                    controllerService.Rumble(200, 0.5f);
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"Controller: {this.ActionType}";
    }
}