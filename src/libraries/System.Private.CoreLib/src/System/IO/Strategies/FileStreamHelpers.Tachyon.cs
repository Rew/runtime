// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Win32.SafeHandles;

namespace System.IO.Strategies
{
    // this type defines a set of stateless FileStream/FileStreamStrategy helper methods
    internal static partial class FileStreamHelpers
    {
        private static OSFileStreamStrategy ChooseStrategyCore(SafeFileHandle handle, FileAccess access, bool isAsync) =>
            throw new NotImplementedException();

       private static FileStreamStrategy ChooseStrategyCore(string path, FileMode mode, FileAccess access, FileShare share, FileOptions options, long preallocationSize, UnixFileMode? unixCreateMode) =>
            throw new NotImplementedException();

        internal static long CheckFileCall(long result, string? path, bool ignoreNotSupported = false)
        {
            throw new NotImplementedException();
        }

        internal static long Seek(SafeFileHandle handle, long offset, SeekOrigin origin, bool closeInvalidHandle = false) => throw new NotImplementedException();

        internal static void ThrowInvalidArgument(SafeFileHandle handle) => throw new NotImplementedException();

        internal static unsafe void SetFileLength(SafeFileHandle handle, long length) => throw new NotImplementedException();

        /// <summary>Flushes the file's OS buffer.</summary>
        internal static void FlushToDisk(SafeFileHandle handle)
        {
            throw new NotImplementedException();
        }

        internal static void Lock(SafeFileHandle handle, bool canWrite, long position, long length)
        {
            throw new NotImplementedException();
        }

        internal static void Unlock(SafeFileHandle handle, long position, long length)
        {
            throw new NotImplementedException();
        }
    }
}
