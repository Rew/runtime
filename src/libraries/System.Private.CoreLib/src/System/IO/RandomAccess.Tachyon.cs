// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Strategies;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
    public static partial class RandomAccess
    {
        internal static long GetFileLength(SafeFileHandle handle) => throw new NotImplementedException();

        internal static unsafe void SetFileLength(SafeFileHandle handle, long length) => throw new NotImplementedException();

        internal static unsafe int ReadAtOffset(SafeFileHandle handle, Span<byte> buffer, long fileOffset) => throw new NotImplementedException();

        internal static unsafe long ReadScatterAtOffset(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers, long fileOffset) => throw new NotImplementedException();

        internal static ValueTask<int> ReadAtOffsetAsync(SafeFileHandle handle, Memory<byte> buffer, long fileOffset, CancellationToken cancellationToken, OSFileStreamStrategy? strategy = null)
            => throw new NotImplementedException();

        private static ValueTask<long> ReadScatterAtOffsetAsync(SafeFileHandle handle, IReadOnlyList<Memory<byte>> buffers,
            long fileOffset, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        internal static unsafe void WriteAtOffset(SafeFileHandle handle, ReadOnlySpan<byte> buffer, long fileOffset) => throw new NotImplementedException();

        internal static unsafe void WriteGatherAtOffset(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers, long fileOffset) => throw new NotImplementedException();

        internal static ValueTask WriteAtOffsetAsync(SafeFileHandle handle, ReadOnlyMemory<byte> buffer, long fileOffset, CancellationToken cancellationToken, OSFileStreamStrategy? strategy = null)
            => throw new NotImplementedException();

        private static ValueTask WriteGatherAtOffsetAsync(SafeFileHandle handle, IReadOnlyList<ReadOnlyMemory<byte>> buffers,
            long fileOffset, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }
}
