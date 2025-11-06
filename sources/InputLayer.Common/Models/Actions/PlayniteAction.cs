using InputLayer.Common.Infrastructures;
using InputLayer.Common.Utils;

namespace InputLayer.Common.Models.Actions
{
    public class PlayniteAction : IAction
    {
        public PlayniteActionType ActionType { get; set; }

        /// <inheritdoc/>
        public void Execute(object obj)
        {
            switch (this.ActionType)
            {
                case PlayniteActionType.ToggleFullscreen:
                    PlayniteLauncher.ToggleFullscreen();
                    break;
                case PlayniteActionType.GoToFullscreen:
                    PlayniteLauncher.GoToFullscreen();
                    break;
                case PlayniteActionType.GoToDesktop:
                    PlayniteLauncher.GoToDesktop();
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"Playnite: {this.ActionType}";
    }
}