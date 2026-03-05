// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace Tachyon
{
    internal static unsafe partial class NativeMemory
    {
        [LibraryImport("*")]
        internal static partial void* Alloc(nuint byteCount);

        [LibraryImport("*")]
        internal static partial void* AllocZeroed(nuint byteCount);

        [LibraryImport("*")]
        internal static partial void Free(void* ptr);

        [LibraryImport("*")]
        internal static partial void* Realloc(void* ptr, nuint byteCount);

        [LibraryImport("*")]
        internal static partial void* AlignedAlloc(nuint alignment, nuint byteCount);

        [LibraryImport("*")]
        internal static partial void AlignedFree(void* ptr);

        [LibraryImport("*")]
        internal static partial void* AlignedRealloc(void* ptr, nuint alignment, nuint byteCount);
    }
}
