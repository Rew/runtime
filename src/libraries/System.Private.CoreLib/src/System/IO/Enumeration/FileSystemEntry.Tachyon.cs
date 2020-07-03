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
        /*private Interop.Sys.DirectoryEntry _directoryEntry;
        private FileStatus _status;
        private Span<char> _pathBuffer;
        private ReadOnlySpan<char> _fullPath;
        private ReadOnlySpan<char> _fileName;
        private fixed char _fileNameBuffer[Interop.Sys.DirectoryEntry.NameBufferSize];

        internal static FileAttributes Initialize(
            ref FileSystemEntry entry,
            Interop.Sys.DirectoryEntry directoryEntry,
            ReadOnlySpan<char> directory,
            ReadOnlySpan<char> rootDirectory,
            ReadOnlySpan<char> originalRootDirectory,
            Span<char> pathBuffer)
        {
            entry._directoryEntry = directoryEntry;
            entry.Directory = directory;
            entry.RootDirectory = rootDirectory;
            entry.OriginalRootDirectory = originalRootDirectory;
            entry._pathBuffer = pathBuffer;
            entry._fullPath = ReadOnlySpan<char>.Empty;
            entry._fileName = ReadOnlySpan<char>.Empty;
            entry._status.InvalidateCaches();
            entry._status.InitiallyDirectory = false;

            bool isDirectory = directoryEntry.InodeType == Interop.Sys.NodeType.DT_DIR;
            bool isSymlink   = directoryEntry.InodeType == Interop.Sys.NodeType.DT_LNK;
            bool isUnknown   = directoryEntry.InodeType == Interop.Sys.NodeType.DT_UNKNOWN;

            if (isDirectory)
            {
                entry._status.InitiallyDirectory = true;
            }
            else if (isSymlink)
            {
                entry._status.InitiallyDirectory = entry._status.IsDirectory(entry.FullPath, continueOnError: true);
            }
            else if (isUnknown)
            {
                entry._status.InitiallyDirectory = entry._status.IsDirectory(entry.FullPath, continueOnError: true);
                if (entry._status.IsSymbolicLink(entry.FullPath, continueOnError: true))
                {
                    entry._directoryEntry.InodeType = Interop.Sys.NodeType.DT_LNK;
                }
            }

            FileAttributes attributes = default;
            if (entry.IsSymbolicLink)
                attributes |= FileAttributes.ReparsePoint;
            if (entry.IsDirectory)
                attributes |= FileAttributes.Directory;

            return attributes;
        }*/

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
    }
}
