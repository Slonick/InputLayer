using System;
using System.IO;
using System.Reflection;

namespace InputLayer.Common.Constants
{
    public static class PathConstants
    {
        public static string AgentFile => Path.Combine(PluginFolder, "InputLayer.Agent.exe");

        public static string LogFile => Path.Combine(PluginFolder, $"input_layer_{Environment.UserName.ToLower()}.log");

        public static string PluginFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}