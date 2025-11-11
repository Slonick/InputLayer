using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using InputLayer.Common.Constants;
using Playnite.SDK;
using ILogger = InputLayer.Common.Logging.ILogger;
using LogManager = InputLayer.Common.Logging.LogManager;

namespace InputLayer.Common.Utils
{
    public static class PlayniteLauncher
    {
        private static readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();

        public static void GoToDesktop()
        {
            var arguments = new[] { "--hidesplashscreen" };
            LaunchPlaynite(arguments);
        }

        public static void GoToFullscreen()
        {
            var arguments = new[] { "--hidesplashscreen", "--startfullscreen" };
            LaunchPlaynite(arguments);
        }

        public static void ToggleFullscreen()
        {
            var isFullscreen = WindowHelper.IsWindowVisible(PlayniteConstants.PlayniteTitle);
            _logger.Info($"Launching Playnite in {(isFullscreen ? "fullscreen" : "desktop")} mode.");

            var arguments = new List<string> { "--hidesplashscreen" };

            if (isFullscreen && API.Instance.ApplicationInfo.Mode == ApplicationMode.Desktop)
            {
                arguments.Add("--startfullscreen");
            }

            LaunchPlaynite(arguments.ToArray());
        }

        private static void LaunchPlaynite(params string[] arguments)
        {
            try
            {
                var playniteExe = Path.Combine(API.Instance.Paths.ApplicationPath, "Playnite.DesktopApp.exe");

                if (!File.Exists(playniteExe))
                {
                    API.Instance.Notifications.Add(new NotificationMessage("executable_not_found",
                                                                           $"Executable not found: {playniteExe}",
                                                                           NotificationType.Error));

                    return;
                }

                var psi = new ProcessStartInfo
                {
                    FileName = playniteExe,
                    Arguments = string.Join(" ", arguments),
                    UseShellExecute = false,
                    WorkingDirectory = API.Instance.Paths.ApplicationPath
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                API.Instance.Notifications.Add(new NotificationMessage("executable_launch_error",
                                                                       $"Error launching executable: {ex.Message}",
                                                                       NotificationType.Error));
            }
        }
    }
}