using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InputLayer.Common.Logging.Layouts
{
    [LogLayout("assembly-version")]
    internal sealed class AssemblyVersionLayout : ILogLayout
    {
        private const string DefaultFormat = "major.minor.build.revision";

        private string _assemblyVersion;

        public void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat)
        {
            string result;
            if ((result = _assemblyVersion) == null)
            {
                result = _assemblyVersion = this.ApplyFormatToVersion(this.GetVersion(logEvent.Assembly), string.IsNullOrWhiteSpace(layoutFormat) ? DefaultFormat : layoutFormat);
            }

            builder.Append(result);
        }

        private string ApplyFormatToVersion(string version, string format)
        {
            if (format.Equals("major.minor.build.revision", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(version))
            {
                return version;
            }

            var array = version.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            version = format.Replace("major", array[0])
                            .Replace("minor", array.Length > 1 ? array[1] : "0")
                            .Replace("build", array.Length > 2 ? array[2] : "0")
                            .Replace("revision", array.Length > 3 ? array[3] : "0");

            return version;
        }

        private Assembly GetAssembly() => Assembly.GetEntryAssembly();

        private string GetVersion(Assembly assembly) => this.GetVersionFromAssembly(assembly ?? this.GetAssembly());

        private string GetVersionFromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            var assemblyFileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return assemblyFileVersion?.Version;
        }
    }
}