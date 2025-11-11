using System.Windows;
using System.Windows.Controls;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;

namespace InputLayer.TemplateSelectors
{
    public class GameControllerActionSettingsSelector : DataTemplateSelector
    {
        public DataTemplate RumbleTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is GameControllerAction action)
            {
                switch (action.ActionType)
                {
                    case GameControllerActionType.Rumble:
                        return this.RumbleTemplate;
                    default:
                        return base.SelectTemplate(item, container);
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}