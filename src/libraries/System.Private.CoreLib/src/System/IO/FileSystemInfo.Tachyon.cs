// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO
{
    public partial class FileSystemInfo
    {
        protected FileSystemInfo() => throw new NotImplementedException();

        internal void InvalidateCore() => throw new NotImplementedException();

        public FileAttributes Attributes
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        internal bool ExistsCore => throw new NotImplementedException();

        internal DateTimeOffset CreationTimeCore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        internal DateTimeOffset LastAccessTimeCore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        internal DateTimeOffset LastWriteTimeCore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        internal long LengthCore => throw new NotImplementedException();

        public void Refresh()
        {
            throw new NotImplementedException();
        }

#pragma warning disable CA1822
        internal UnixFileMode UnixFileModeCore
        {
            get => (UnixFileMode)(-1);
            set => throw new PlatformNotSupportedException(SR.PlatformNotSupported_UnixFileMode);
        }
#pragma warning restore CA1822

        internal string NormalizedPath => throw new NotImplementedException();
    }
}
