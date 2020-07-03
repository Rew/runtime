// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;

namespace System.Runtime.InteropServices
{
    public static partial class Marshal
    {
        public static string? PtrToStringAuto(IntPtr ptr, int len)
        {
            throw new NotImplementedException();
        }

        public static string? PtrToStringAuto(IntPtr ptr)
        {
            throw new NotImplementedException();
        }

        public static IntPtr StringToHGlobalAuto(string? s)
        {
            throw new NotImplementedException();
        }

        public static IntPtr StringToCoTaskMemAuto(string? s)
        {
            throw new NotImplementedException();
        }

        private static int GetSystemMaxDBCSCharSize() => 3;

        private static bool IsNullOrWin32Atom(IntPtr ptr) => ptr == IntPtr.Zero;

        internal static unsafe int StringToAnsiString(string s, byte* buffer, int bufferLength, bool bestFit = false, bool throwOnUnmappableChar = false)
        {
            throw new NotImplementedException();
        }

        // Returns number of bytes required to convert given string to Ansi string. The return value includes null terminator.
        internal static unsafe int GetAnsiStringByteCount(ReadOnlySpan<char> chars)
        {
            throw new NotImplementedException();
        }

        // Converts given string to Ansi string. The destination buffer must be large enough to hold the converted value, including null terminator.
        internal static unsafe void GetAnsiStringBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            throw new NotImplementedException();
        }
    }
}
