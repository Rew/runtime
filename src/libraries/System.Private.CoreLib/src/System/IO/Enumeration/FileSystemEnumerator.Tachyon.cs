// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.IO.Enumeration
{
    public abstract unsafe partial class FileSystemEnumerator<TResult> : CriticalFinalizerObject, IEnumerator<TResult>
    {
        private readonly string _originalRootDirectory;
        private readonly string _rootDirectory;
        private readonly EnumerationOptions _options;
        private bool _lastEntryFound;
        private TResult? _current;
        private string? _currentPath;
        private IntPtr _entry;

        public bool MoveNext() { if (_entry != IntPtr.Zero && _lastEntryFound) return false; throw new NotImplementedException(); }

        private void Init() => throw new NotImplementedException();

        internal FileSystemEnumerator(string directory, bool isNormalized, EnumerationOptions? options, string? expression) :
            this(directory, isNormalized, options)
        {
            _ = expression; // unused
        }

        private IntPtr CreateDirectoryHandle(string path, bool ignoreNotFound = false) => throw new NotImplementedException();

        private void CloseDirectoryHandle() => throw new NotImplementedException();

        private unsafe void FindNextEntry() => throw new NotImplementedException();

        private bool DequeueNextDirectory() => throw new NotImplementedException();

        private void InternalDispose(bool disposing) => throw new NotImplementedException();
    }
}
