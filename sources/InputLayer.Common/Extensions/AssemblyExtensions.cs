using System;
using System.Reflection;

namespace InputLayer.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static Assembly GetAssembly(this Type type)
            => type.GetTypeInfo().Assembly;

        public static T GetAttributeOfType<T>(this Type type) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
    }
}