using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using InputLayer.Common.Extensions;

namespace InputLayer.Common.Utils
{
    public static class StackTraceUtils
    {
        private static readonly Assembly _mscorlibAssembly = typeof(string).GetAssembly();
        private static readonly Assembly _systemAssembly = typeof(Debug).GetAssembly();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetClassFullName()
        {
            const int skipFrames = 2;
            return GetClassFullName(new StackFrame(skipFrames, false));
        }

        public static string GetClassFullName(StackFrame stackFrame)
        {
            var text = LookupClassNameFromStackFrame(stackFrame);
            if (string.IsNullOrEmpty(text) || text == typeof(StackTraceUtils).FullName)
            {
                text = GetClassFullName(new StackTrace(false));
            }

            return text;
        }

        public static string GetStackFrameMethodClassName(MethodBase method, bool includeNameSpace, bool cleanAsyncMoveNext, bool cleanAnonymousDelegates)
        {
            if (method == null)
            {
                return null;
            }

            var declaringType = method.DeclaringType;
            if (cleanAsyncMoveNext && method.Name == "MoveNext" && declaringType?.DeclaringType != null && declaringType.Name.StartsWith("<") && declaringType.Name.IndexOf('>', 1) > 1)
            {
                declaringType = declaringType.DeclaringType;
            }

            if (!includeNameSpace && declaringType?.DeclaringType != null && declaringType.IsNested && declaringType.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
            {
                return declaringType.DeclaringType?.Name;
            }

            var text = includeNameSpace ? declaringType?.FullName : declaringType?.Name;
            if (cleanAnonymousDelegates && text != null)
            {
                var num = text.IndexOf("+<>", StringComparison.Ordinal);
                if (num >= 0)
                {
                    text = text.Substring(0, num);
                }
            }

            return text;
        }

        public static Assembly LookupAssemblyFromStackFrame(StackFrame stackFrame)
        {
            var method = stackFrame.GetMethod();
            if (method == null)
            {
                return null;
            }

            var declaringType = method.DeclaringType;
            var assembly = declaringType?.GetAssembly();
            if (assembly == null)
            {
                var module = method.Module;
                assembly = module.Assembly;
            }

            if (assembly == _mscorlibAssembly)
            {
                return null;
            }

            if (assembly == _systemAssembly)
            {
                return null;
            }

            return assembly;
        }

        public static string LookupClassNameFromStackFrame(StackFrame stackFrame)
        {
            var method = stackFrame.GetMethod();
            if (method != null && LookupAssemblyFromStackFrame(stackFrame) != null)
            {
                var text = GetStackFrameMethodClassName(method, true, true, true) ?? method.Name;
                if (!string.IsNullOrEmpty(text) && !text.StartsWith("System.", StringComparison.Ordinal))
                {
                    return text;
                }
            }

            return string.Empty;
        }

        private static string GetClassFullName(StackTrace stackTrace)
        {
            var frames = stackTrace.GetFrames();
            if (frames == null)
            {
                return string.Empty;
            }

            foreach (var frame in frames)
            {
                var text = LookupClassNameFromStackFrame(frame);
                if (!string.IsNullOrEmpty(text) && text != typeof(StackTraceUtils).FullName)
                {
                    return text;
                }
            }

            return string.Empty;
        }
    }
}