// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace System.IO.Enumeration
{
    /// <summary>
    /// Lower level view of FileSystemInfo used for processing and filtering find results.
    /// </summary>
    public unsafe ref partial struct FileSystemEntry
    {
        private ReadOnlySpan<char> FullPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ReadOnlySpan<char> FileName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The full path of the directory this entry resides in.
        /// </summary>
        public ReadOnlySpan<char> Directory { get; private set; }

        /// <summary>
        /// The full path of the root directory used for the enumeration.
        /// </summary>
        public ReadOnlySpan<char> RootDirectory { get; private set; }

        /// <summary>
        /// The root directory for the enumeration as specified in the constructor.
        /// </summary>
        public ReadOnlySpan<char> OriginalRootDirectory { get; private set; }

        // Windows never fails getting attributes, length, or time as that information comes back
        // with the native enumeration struct. As such we must not throw here.
        public FileAttributes Attributes
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public long Length => throw new NotImplementedException();
        public DateTimeOffset CreationTimeUtc => throw new NotImplementedException();
        public DateTimeOffset LastAccessTimeUtc => throw new NotImplementedException();
        public DateTimeOffset LastWriteTimeUtc => throw new NotImplementedException();
        public bool IsHidden => throw new NotImplementedException();
        internal bool IsReadOnly => throw new NotImplementedException();

        public bool IsDirectory => throw new NotImplementedException();
        internal bool IsSymbolicLink => throw new NotImplementedException();

        public FileSystemInfo ToFileSystemInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the full path of the find result.
        /// </summary>
        public string ToFullPath() => throw new NotImplementedException();

        private static string Join(
            ReadOnlySpan<char> originalRootDirectory,
            ReadOnlySpan<char> relativePath,
            ReadOnlySpan<char> fileName) =>
            Path.Join(originalRootDirectory, relativePath, fileName);
    }
}
