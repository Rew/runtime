// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Win32.SafeHandles
{
    public sealed class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeFileHandle() : this(ownsHandle: true)
        {
        }

        private SafeFileHandle(bool ownsHandle)
            : base(ownsHandle)
        {
                throw new NotImplementedException();
        }

        public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle) : this(ownsHandle)
        {
                throw new NotImplementedException();
        }

        internal bool? IsAsync { get; set; }

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
    }
}
