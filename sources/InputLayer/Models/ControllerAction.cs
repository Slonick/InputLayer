using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using InputLayer.Common.Infrastructures;
using Newtonsoft.Json;
using Playnite.SDK;
using ILogger = InputLayer.Common.Logging.ILogger;
using LogManager = InputLayer.Common.Logging.LogManager;

namespace InputLayer.Models
{
    public class ControllerAction : ObservableObject
    {
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        private ObservableCollection<ControllerActionItem> _actions = new ObservableCollection<ControllerActionItem>();

        private ControllerInput _button = ControllerInput.None;
        private ControllerButtonMode _mode = ControllerButtonMode.Combination;

        public ControllerAction()
        {
            this.AddControllerActionItemCommand = new RelayCommand<ControllerActionItem>(this.AddControllerActionItem);
            this.RemoveControllerActionItemCommand = new RelayCommand<ControllerActionItem>(this.RemoveControllerActionItem, this.RemoveControllerActionItemCanExecute);
            this.UpControllerActionItemCommand = new RelayCommand<ControllerActionItem>(this.UpControllerActionItem, this.UpControllerActionItemCanExecute);
            this.DownControllerActionItemCommand = new RelayCommand<ControllerActionItem>(this.DownControllerActionItem, this.DownControllerActionItemCanExecute);
        }

        [JsonIgnore]
        public ICommand AddControllerActionItemCommand { get; }

        [JsonIgnore]
        public ICommand DownControllerActionItemCommand { get; }

        [JsonIgnore]
        public ICommand RemoveControllerActionItemCommand { get; }

        [JsonIgnore]
        public ICommand UpControllerActionItemCommand { get; }

        public ObservableCollection<ControllerActionItem> Actions
        {
            get => _actions;
            set => this.SetValue(ref _actions, value);
        }

        public ControllerInput Button
        {
            get => _button;
            set => this.SetValue(ref _button, value);
        }

        public bool IsPredefined { get; set; }

        public ControllerButtonMode Mode
        {
            get => _mode;
            set => this.SetValue(ref _mode, value);
        }

        internal static ControllerAction Default() => new ControllerAction
        {
            Actions = new ObservableCollection<ControllerActionItem>
            {
                ControllerActionItem.Default()
            }
        };

        private void AddControllerActionItem(ControllerActionItem item)
        {
            var index = this.Actions.IndexOf(item);
            this.Actions.Insert(index + 1, ControllerActionItem.Default());
        }

        private void DownControllerActionItem(ControllerActionItem item)
        {
            var index = this.Actions.IndexOf(item);
            this.Actions.Move(index, index + 1);
        }

        private bool DownControllerActionItemCanExecute(ControllerActionItem item)
        {
            try
            {
                return this.Actions.IndexOf(item) < this.Actions.Count - 1;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error");
            }

            return true;
        }

        private void RemoveControllerActionItem(ControllerActionItem item)
            => this.Actions.Remove(item);

        private bool RemoveControllerActionItemCanExecute(ControllerActionItem item)
            => this.Actions.Count > 1;

        private void UpControllerActionItem(ControllerActionItem item)
        {
            var index = this.Actions.IndexOf(item);
            this.Actions.Move(index, index - 1);
        }

        private bool UpControllerActionItemCanExecute(ControllerActionItem item)
            => this.Actions.IndexOf(item) > 0;
    }
}