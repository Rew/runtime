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

        private IntPtr CreateDirectoryHandle(string path, bool ignoreNotFound = false) => throw new NotImplementedException();

        private void CloseDirectoryHandle() => throw new NotImplementedException();

        private unsafe void FindNextEntry() => throw new NotImplementedException();

        private unsafe void FindNextEntry(byte* entryBufferPtr, int bufferLength) => throw new NotImplementedException();

        private bool DequeueNextDirectory() => throw new NotImplementedException();

        private void InternalDispose(bool disposing) => throw new NotImplementedException();

        // The largest supported path on Unix is 4K bytes of UTF-8 (most only support 1K)
        /*private const int StandardBufferSize = 4096;

        private readonly object _lock = new object();

        private IntPtr _directoryHandle;
        private Queue<(string Path, int RemainingDepth)>? _pending;


        // Used for creating full paths
        private char[]? _pathBuffer;
        // Used to get the raw entry data
        private byte[]? _entryBuffer;

*/
    }
}
