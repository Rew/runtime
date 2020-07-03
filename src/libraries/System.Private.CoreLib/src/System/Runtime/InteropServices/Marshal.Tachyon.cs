// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;

namespace System.Runtime.InteropServices
{
    public static partial class Marshal
    {
        public static IntPtr AllocCoTaskMem(int cb) => throw new NotImplementedException();

        public static unsafe IntPtr ReAllocCoTaskMem(IntPtr pv, int cb) => throw new NotImplementedException();

        public static void FreeCoTaskMem(IntPtr ptr) => throw new NotImplementedException();

        internal static unsafe IntPtr AllocBSTR(int length) => throw new NotImplementedException();

        internal static unsafe IntPtr AllocBSTRByteLen(uint length) => throw new NotImplementedException();

        public static unsafe void FreeBSTR(IntPtr ptr) => throw new NotImplementedException();

        internal static Type? GetTypeFromProgID(string progID, string? server, bool throwOnError) => throw new NotImplementedException();

        public static unsafe IntPtr AllocHGlobal(IntPtr cb) => throw new NotImplementedException();

        public static unsafe IntPtr ReAllocHGlobal(IntPtr pv, IntPtr cb) => throw new NotImplementedException();

        public static unsafe void FreeHGlobal(IntPtr hglobal) => throw new NotImplementedException();

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

        /// <summary>
        /// Get the last system error on the current thread
        /// </summary>
        /// <returns>The last system error</returns>
        /// <remarks>
        /// The error is that for the current operating system (e.g. errno on Unix, GetLastError on Windows)
        /// </remarks>
        public static int GetLastSystemError()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the last system error on the current thread
        /// </summary>
        /// <param name="error">Error to set</param>
        /// <remarks>
        /// The error is that for the current operating system (e.g. errno on Unix, SetLastError on Windows)
        /// </remarks>
        public static void SetLastSystemError(int error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the system error message for the supplied error code.
        /// </summary>
        /// <param name="error">The error code.</param>
        /// <returns>The error message associated with <paramref name="error"/>.</returns>
        public static string GetPInvokeErrorMessage(int error)
        {
            throw new NotImplementedException();
        }
    }
}
