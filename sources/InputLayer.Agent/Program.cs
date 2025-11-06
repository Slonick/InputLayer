using System;
using InputLayer.Common.Logging;
using InputLayer.IPC;
using InputLayer.IPC.Models;

namespace InputLayer.Agent
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Bootstrapper.Setup();

            var logger = LogManager.Default.GetCurrentClassLogger();

            logger.Info("InputLayer.Agent starting...");
            logger.Info($"Command line args: {string.Join(" ", args)}");

            try
            {
                var sdlService = new SDLService();
                var ipcServer = new IPCServer();

                logger.Info("Subscribing to IPC message events...");

                ipcServer.Disconnected += () =>
                {
                    logger.Info("IPC client disconnected. Exiting.");
                    Environment.Exit(0);
                };

                ipcServer.MessageReceived += message =>
                {
                    switch (message)
                    {
                        case RumbleMessage rumbleMessage:
                            sdlService.Rumble(rumbleMessage.DurationMs, rumbleMessage.Intensity);
                            break;
                    }
                };

                logger.Info("Connecting to IPC server...");
                ipcServer.Connect();
                logger.Info("IPC server connected.");

                ipcServer.Send(new PingMessage());

                logger.Info("Subscribing to SDL service events...");
                sdlService.ControllerConnected += () =>
                {
                    logger.Info("Controller connected. Sending IPC notification.");
                    ipcServer.Send(new ControllerConnectedMessage());
                };

                sdlService.ControllerDisconnected += () =>
                {
                    logger.Info("Controller disconnected. Sending IPC notification.");
                    ipcServer.Send(new ControllerDisconnectedMessage());
                };

                sdlService.ButtonPressed += button => ipcServer.Send(new ButtonPressedMessage(button));

                sdlService.ButtonReleased += button => ipcServer.Send(new ButtonReleasedMessage(button));

                logger.Info("InputLayer.Agent started successfully.");
                Console.ReadKey();

                logger.Info("Shutting down InputLayer.Agent...");
            }
            catch (Exception e)
            {
                logger.Error(e, "Unhandled exception in main loop.");
                Console.ReadKey();
                throw;
            }
        }
    }
}