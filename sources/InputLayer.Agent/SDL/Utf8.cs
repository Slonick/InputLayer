using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable All
namespace InputLayer.Agent
{
    public static partial class SDL
    {
        #region UTF8 Marshaling

        internal static int Utf8Size(string str)
        {
            if (str == null)
            {
                return 0;
            }

            return str.Length * 4 + 1;
        }

        internal static unsafe byte* Utf8EncodeHeap(string str)
        {
            if (str == null)
            {
                return (byte*)0;
            }

            var bufferSize = Utf8Size(str);
            var buffer = (byte*)Marshal.AllocHGlobal(bufferSize);
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }

            return buffer;
        }

        public static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero)
            {
                return null;
            }

            var ptr = (byte*)s;
            while (*ptr != 0)
            {
                ptr++;
            }

            var len = (int)(ptr - (byte*)s);
            if (len == 0)
            {
                return string.Empty;
            }

            var chars = stackalloc char[len];
            var strLen = Encoding.UTF8.GetChars((byte*)s, len, chars, len);
            var result = new string(chars, 0, strLen);

            if (freePtr)
            {
                SDL_free(s);
            }

            return result;
        }

        #endregion
    }
}