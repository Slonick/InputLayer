using System.Collections.Generic;
using System.Linq;
using InputLayer.Keyboard;

namespace InputLayer.Common.Models.Actions
{
    public class KeyboardAction : ObservableObject, IAction
    {
        private Keys _key;
        private Modifiers[] _modifiers;

        public Keys Key
        {
            get => _key;
            set => this.SetValue(ref _key, value);
        }

        public Modifiers[] Modifiers
        {
            get => _modifiers;
            set => this.SetValue(ref _modifiers, value);
        }

        /// <inheritdoc/>
        public void Execute(object obj)
        {
            KeyboardSimulator.KeyPress(this.Modifiers, this.Key);
        }

        /// <inheritdoc/>
        public override string ToString() => this.Modifiers.Any() ? $"Keyboard: {string.Join("+", this.Modifiers)}+{this.Key}" : $"Keyboard: {this.Key}";
    }
}