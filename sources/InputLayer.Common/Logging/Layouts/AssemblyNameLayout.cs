using System.Reflection;
using System.Text;

namespace InputLayer.Common.Logging.Layouts
{
    [LogLayout("assembly-name")]
    internal sealed class AssemblyNameLayout : ILogLayout
    {
        private string _assemblyName;

        public void Append(StringBuilder builder, LogEventInfo logEvent, string layoutFormat)
        {
            string result;
            if ((result = _assemblyName) == null)
            {
                result = _assemblyName = this.GetName(logEvent.Assembly);
            }

            builder.Append(result);
        }

        private Assembly GetAssembly() => Assembly.GetEntryAssembly();

        private string GetName(Assembly assembly) => this.GetNameFromAssembly(assembly ?? this.GetAssembly());

        private string GetNameFromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            var assemblyTitle = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            return assemblyTitle?.Title;
        }
    }
}