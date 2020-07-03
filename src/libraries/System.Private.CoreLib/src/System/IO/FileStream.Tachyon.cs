// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    /// <summary>Provides an implementation of a file stream for Unix files.</summary>
    public partial class FileStream : Stream
    {
        private SafeFileHandle OpenHandle(FileMode mode, FileShare share, FileOptions options)
        {
            throw new NotImplementedException();
        }

        private static bool GetDefaultIsAsync(SafeFileHandle handle) => throw new NotImplementedException();

        /// <summary>Initializes a stream for reading or writing a Unix file.</summary>
        /// <param name="mode">How the file should be opened.</param>
        /// <param name="share">What other access to the file should be allowed.  This is currently ignored.</param>
        /// <param name="originalPath">The original path specified for the FileStream.</param>
        private void Init(FileMode mode, FileShare share, string originalPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>Initializes a stream from an already open file handle (file descriptor).</summary>
        private void InitFromHandle(SafeFileHandle handle, FileAccess access, bool useAsyncIO)
        {
            throw new NotImplementedException();
        }

/*
        /// <summary>Translates the FileMode, FileAccess, and FileOptions values into flags to be passed when opening the file.</summary>
        /// <param name="mode">The FileMode provided to the stream's constructor.</param>
        /// <param name="access">The FileAccess provided to the stream's constructor</param>
        /// <param name="share">The FileShare provided to the stream's constructor</param>
        /// <param name="options">The FileOptions provided to the stream's constructor</param>
        /// <returns>The flags value to be passed to the open system call.</returns>
        private static Interop.Sys.OpenFlags PreOpenConfigurationFromOptions(FileMode mode, FileAccess access, FileShare share, FileOptions options)
        {
            // Translate FileMode.  Most of the values map cleanly to one or more options for open.
            Interop.Sys.OpenFlags flags = default;
            switch (mode)
            {
                default:
                case FileMode.Open: // Open maps to the default behavior for open(...).  No flags needed.
                case FileMode.Truncate: // We truncate the file after getting the lock
                    break;

                case FileMode.Append: // Append is the same as OpenOrCreate, except that we'll also separately jump to the end later
                case FileMode.OpenOrCreate:
                case FileMode.Create: // We truncate the file after getting the lock
                    flags |= Interop.Sys.OpenFlags.O_CREAT;
                    break;

                case FileMode.CreateNew:
                    flags |= (Interop.Sys.OpenFlags.O_CREAT | Interop.Sys.OpenFlags.O_EXCL);
                    break;
            }

            // Translate FileAccess.  All possible values map cleanly to corresponding values for open.
            switch (access)
            {
                case FileAccess.Read:
                    flags |= Interop.Sys.OpenFlags.O_RDONLY;
                    break;

                case FileAccess.ReadWrite:
                    flags |= Interop.Sys.OpenFlags.O_RDWR;
                    break;

                case FileAccess.Write:
                    flags |= Interop.Sys.OpenFlags.O_WRONLY;
                    break;
            }

            // Handle Inheritable, other FileShare flags are handled by Init
            if ((share & FileShare.Inheritable) == 0)
            {
                flags |= Interop.Sys.OpenFlags.O_CLOEXEC;
            }

            // Translate some FileOptions; some just aren't supported, and others will be handled after calling open.
            // - Asynchronous: Handled in ctor, setting _useAsync and SafeFileHandle.IsAsync to true
            // - DeleteOnClose: Doesn't have a Unix equivalent, but we approximate it in Dispose
            // - Encrypted: No equivalent on Unix and is ignored
            // - RandomAccess: Implemented after open if posix_fadvise is available
            // - SequentialScan: Implemented after open if posix_fadvise is available
            // - WriteThrough: Handled here
            if ((options & FileOptions.WriteThrough) != 0)
            {
                flags |= Interop.Sys.OpenFlags.O_SYNC;
            }

            return flags;
        }*/

        private void LockInternal(long position, long length)
        {
            throw new NotImplementedException();
        }

        private void UnlockInternal(long position, long length)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
        public override bool CanSeek => CanSeekCore(_fileHandle);

        /// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
        /// <remarks>
        /// Separated out of CanSeek to enable making non-virtual call to this logic.
        /// We also pass in the file handle to allow the constructor to use this before it stashes the handle.
        /// </remarks>
        private bool CanSeekCore(SafeFileHandle fileHandle)
        {
            throw new NotImplementedException();
        }

        private long GetLengthInternal()
        {
            throw new NotImplementedException();
        }

        /// <summary>Releases the unmanaged resources used by the stream.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public override ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>Flushes the OS buffer.  This does not flush the internal read/write buffer.</summary>
        private void FlushOSBuffer()
        {
            throw new NotImplementedException();
        }

        private void FlushWriteBufferForWriteByte()
        {
            throw new NotImplementedException();
        }

        /// <summary>Writes any data in the write buffer to the underlying stream and resets the buffer.</summary>
        private void FlushWriteBuffer()
        {
            throw new NotImplementedException();
        }

        /// <summary>Asynchronously clears all buffers for this stream, causing any buffered data to be written to the underlying device.</summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous flush operation.</returns>
        private Task FlushAsyncInternal(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>Sets the length of this stream to the given value.</summary>
        /// <param name="value">The new length of the stream.</param>
        private void SetLengthInternal(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>Reads a block of bytes from the stream and writes the data in a given buffer.</summary>
        private int ReadSpan(Span<byte> destination)
        {
            throw new NotImplementedException();
        }

        /// <summary>Unbuffered, reads a block of bytes from the file handle into the given buffer.</summary>
        /// <param name="buffer">The buffer into which data from the file is read.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This might be less than the number of bytes requested
        /// if that number of bytes are not currently available, or zero if the end of the stream is reached.
        /// </returns>
        private unsafe int ReadNative(Span<byte> buffer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream and advances
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="destination">The buffer to write the data into.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="synchronousResult">If the operation completes synchronously, the number of bytes read.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        private Task<int>? ReadAsyncInternal(Memory<byte> destination, CancellationToken cancellationToken, out int synchronousResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>Reads from the file handle into the buffer, overwriting anything in it.</summary>
        private int FillReadBufferForReadByte()
        {
            throw new NotImplementedException();
        }

        /// <summary>Writes a block of bytes to the file stream.</summary>
        /// <param name="source">The buffer containing data to write to the stream.</param>
        private void WriteSpan(ReadOnlySpan<byte> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>Unbuffered, writes a block of bytes to the file stream.</summary>
        /// <param name="source">The buffer containing data to write to the stream.</param>
        private unsafe void WriteNative(ReadOnlySpan<byte> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream, advances
        /// the current position within this stream by the number of bytes written, and
        /// monitors cancellation requests.
        /// </summary>
        /// <param name="source">The buffer to write data from.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        private ValueTask WriteAsyncInternal(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) =>
            // Windows version overrides this method, so the Unix version does as well, but it doesn't
            // currently have any special optimizations to be done and so just calls to the base.
            base.CopyToAsync(destination, bufferSize, cancellationToken);

        /// <summary>Sets the current position of this stream to the given value.</summary>
        /// <param name="offset">The point relative to origin from which to begin seeking. </param>
        /// <param name="origin">
        /// Specifies the beginning, the end, or the current position as a reference
        /// point for offset, using a value of type SeekOrigin.
        /// </param>
        /// <returns>The new position in the stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <summary>Sets the current position of this stream to the given value.</summary>
        /// <param name="fileHandle">The file handle on which to seek.</param>
        /// <param name="offset">The point relative to origin from which to begin seeking. </param>
        /// <param name="origin">
        /// Specifies the beginning, the end, or the current position as a reference
        /// point for offset, using a value of type SeekOrigin.
        /// </param>
        /// <returns>The new position in the stream.</returns>
        private long SeekCore(SafeFileHandle fileHandle, long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        private long CheckFileCall(long result, bool ignoreNotSupported = false)
        {
            throw new NotImplementedException();
        }

        private int CheckFileCall(int result, bool ignoreNotSupported = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>State used when the stream is in async mode.</summary>
        private sealed class AsyncState : SemaphoreSlim
        {
            internal ReadOnlyMemory<byte> ReadOnlyMemory;
            internal Memory<byte> Memory;

            /// <summary>Initialize the AsyncState.</summary>
            internal AsyncState() : base(initialCount: 1, maxCount: 1) { }
        }
    }
}
