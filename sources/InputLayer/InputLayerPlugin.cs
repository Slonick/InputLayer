using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using InputLayer.Common.Extensions;
using InputLayer.Common.Infrastructures;
using InputLayer.Common.Services;
using InputLayer.Models;
using InputLayer.Services;
using InputLayer.ViewModels;
using InputLayer.Views;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using ControllerInput = InputLayer.Common.Infrastructures.ControllerInput;
using ILogger = InputLayer.Common.Logging.ILogger;
using LogManager = InputLayer.Common.Logging.LogManager;

namespace InputLayer
{
    public class InputLayerPlugin : GenericPlugin
    {
        private readonly IControllerService _controllerService;
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private readonly InputLayerSettingsViewModel _model;
        private readonly InputLayerSettings _settings;

        public InputLayerPlugin(IPlayniteAPI playniteApi) : base(playniteApi)
        {
            Bootstrapper.Setup();

            var settingsService = new SettingsManager(this);

            _logger.Trace("Loading plugin settings...");
            var settings = settingsService.LoadPluginSettings();
            if (settings is null || settings.DesktopActions.All(x => x.Mode != ControllerButtonMode.Single))
            {
                _logger.Warn("Failed to load plugin settings, using default settings.");
                settings = InputLayerSettings.Default;
            }

            _settings = settings;

            _controllerService = new ControllerService(_settings);

            _model = new InputLayerSettingsViewModel(settingsService, _settings, _controllerService);
            this.Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }

        /// <inheritdoc/>
        public override Guid Id => Guid.Parse("a4c131df-24fb-44f7-8e79-db5cd3988563");

        /// <inheritdoc/>
        public override ISettings GetSettings(bool firstRunSettings)
            => _model;

        /// <inheritdoc/>
        public override UserControl GetSettingsView(bool firstRunSettings)
            => new InputLayerSettingsView { DataContext = _model };

        /// <inheritdoc/>
        public override async void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            try
            {
                _logger.Trace("InputLayer started.");

                await _controllerService.InitializeAsync();

                if (PlayniteApi.ApplicationInfo.Mode == ApplicationMode.Fullscreen)
                {
                    _logger.Info("Subscribing to button events for Fullscreen mode.");
                    _controllerService.ButtonCombinationPressed += this.ButtonCombinationPressed;
                }
                else
                {
                    _logger.Info("Subscribing to button events for Desktop mode.");
                    _controllerService.ButtonReleased += this.OnButtonReleasedDesktop;
                    _controllerService.ButtonCombinationPressed += this.ButtonCombinationPressed;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error starting InputLayer.");
            }
        }

        /// <inheritdoc/>
        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            _logger.Trace("InputLayer stopping...");
            _controllerService.Dispose();
            _logger.Trace("InputLayer stopped.");
        }

        private void ButtonCombinationPressed(IReadOnlyList<ControllerInput> combination)
        {
            _logger.Trace($"Button combination pressed: {string.Join(" | ", combination)}");

            if (_model.IsEditing)
            {
                _logger.Debug("Ignoring button press - settings are being edited.");
                return;
            }

            if (!combination.ContainsButtons(_settings.MainButton))
            {
                _logger.Debug("Button combination does not contain main button.");
                return;
            }

            var source = (PlayniteApi.ApplicationInfo.Mode == ApplicationMode.Fullscreen
                    ? _settings.FullScreenActions
                    : _settings.DesktopActions)
                .Where(x => x.Mode == ControllerButtonMode.Combination);

            var gameOpened = PlayniteApi.Database.Games.Any(g => g.IsLaunching || g.IsRunning);
            if (gameOpened)
            {
                source = _settings.InGameActions.Where(x => x.Mode == ControllerButtonMode.Combination);
                _logger.Debug("Game is running. Using in-game actions.");
            }

            var button = combination.ExceptButtons(_settings.MainButton).Single();
            _logger.Debug($"Button: {button}");

            var controllerAction = source.SingleOrDefault(x => x.Button == button);
            _logger.Debug($"Controller action: {controllerAction}");

            if (controllerAction != null)
            {
                foreach (var action in controllerAction.Actions)
                {
                    _logger.Trace($"Executing action: {action.Action}");
                    switch (action.ActionType)
                    {
                        case ActionType.GameController:
                            action.Action.Execute(_controllerService);
                            break;
                        default:
                            action.Action.Execute();
                            break;
                    }
                }
            }
        }

        private void OnButtonReleasedDesktop(ControllerInput button)
        {
            _logger.Trace($"Button released in Desktop mode: {button}");

            if (_model.IsEditing)
            {
                _logger.Debug("Ignoring button press - settings are being edited.");
                return;
            }

            var gameOpened = PlayniteApi.Database.Games.Any(g => g.IsLaunching || g.IsRunning);
            if (gameOpened)
            {
                _logger.Debug("Game is running. Ignoring button press.");
                return;
            }

            if (button == _settings.MainButton)
            {
                button = ControllerInput.Main;
            }

            var controllerAction = _settings.DesktopActions
                                            .Where(x => x.Mode == ControllerButtonMode.Single)
                                            .SingleOrDefault(x => x.Button == button);

            _logger.Debug($"Controller action: {controllerAction}");

            if (controllerAction != null)
            {
                foreach (var action in controllerAction.Actions)
                {
                    try
                    {
                        _logger.Trace($"Executing action: {action.Action}");
                        action.Action.Execute();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"Failed to execute action: {action.Action}");
                    }
                }
            }
        }
    }
}