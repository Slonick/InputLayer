using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InputLayer.Common.Extensions
{
    public static class AssemblyExtension
    {
        public static string AssemblyDirectory(this Assembly assembly)
        {
            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static Assembly GetAssembly(this Type type) => type.GetTypeInfo().Assembly;

        public static T GetAssemblyAttribute<T>(this Assembly assembly) where T : Attribute =>
            (T)assembly?.GetCustomAttributes(typeof(T), true).FirstOrDefault();

        /// <summary>
        ///     Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="type">The type of object</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Type type) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
    }
}