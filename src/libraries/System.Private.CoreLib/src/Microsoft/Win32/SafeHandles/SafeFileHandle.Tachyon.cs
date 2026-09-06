// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Microsoft.Win32.SafeHandles
{
    public sealed partial class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeFileHandle() : base(true)
        {
                throw new NotImplementedException();
        }

        public bool IsAsync => throw new NotImplementedException();

        public static partial void CreateAnonymousPipe(out SafeFileHandle readHandle, out SafeFileHandle writeHandle, bool asyncRead, bool asyncWrite)
        {
                throw new NotImplementedException();
        }

        /*/// <summary>Opens the specified file with the requested flags and mode.</summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="flags">The flags with which to open the file.</param>
        /// <param name="mode">The mode for opening the file.</param>
        /// <returns>A SafeFileHandle for the opened file.</returns>
        internal static SafeFileHandle Open(string path, Interop.Sys.OpenFlags flags, int mode)
        {
                throw new NotImplementedException();
        }

        private static bool DirectoryExists(string fullPath)
        {
                throw new NotImplementedException();
        }*/

#pragma warning disable CA1822
        internal ThreadPoolBoundHandle? ThreadPoolBinding => null;

        internal void EnsureThreadPoolBindingInitialized() { throw new NotImplementedException(); }
#pragma warning restore CA1822
        internal static unsafe SafeFileHandle Open(string fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options, long preallocationSize, UnixFileMode? unixCreateMode = null)
        {
                throw new NotImplementedException();
        }

        /// <summary>Opens a SafeFileHandle for a file descriptor created by a provided delegate.</summary>
        /// <param name="fdFunc">
        /// The function that creates the file descriptor. Returns the file descriptor on success, or an invalid
        /// file descriptor on error with Marshal.GetLastWin32Error() set to the error code.
        /// </param>
        /// <returns>The created SafeFileHandle.</returns>
        internal static SafeFileHandle Open(Func<SafeFileHandle> fdFunc)
        {
                throw new NotImplementedException();
        }

        protected override bool ReleaseHandle()
        {
                throw new NotImplementedException();
        }

        public override bool IsInvalid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal long GetFileLength()
        {
                throw new NotImplementedException();
        }

        internal bool TryGetCachedLength(out long cachedLength)
        {
                throw new NotImplementedException();
        }

        internal System.IO.FileHandleType GetFileTypeCore()
        {
                throw new NotImplementedException();
        }

        private bool GetCanSeekCore()
        {
                throw new NotImplementedException();
        }
    }
}
