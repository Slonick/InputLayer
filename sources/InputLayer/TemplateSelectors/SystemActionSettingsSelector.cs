using System.Windows;
using System.Windows.Controls;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Models.Actions;

namespace InputLayer.TemplateSelectors
{
    public class SystemActionSettingsSelector : DataTemplateSelector
    {
        public DataTemplate PauseTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SystemAction action)
            {
                switch (action.ActionType)
                {
                    case SystemActionType.Pause:
                        return this.PauseTemplate;
                    default:
                        return base.SelectTemplate(item, container);
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}