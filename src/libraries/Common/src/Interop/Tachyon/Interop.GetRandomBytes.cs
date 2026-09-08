// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Sys
    {
        // A host service: on Tachyon every non-wildcard import is an extern symbol named
        // by its entry point, which the host provides (the AOT harness's services.c, the
        // JIT host's RegisterInterop). The name is the System.Native PAL's, so a host that
        // already has the PAL needs nothing new. HashCode's static constructor reaches
        // this at startup for its seed.
        [LibraryImport(Interop.Libraries.SystemNative, EntryPoint = "SystemNative_GetNonCryptographicallySecureRandomBytes")]
        internal static unsafe partial void GetNonCryptographicallySecureRandomBytes(byte* buffer, int length);
    }

    internal static unsafe void GetRandomBytes(byte* buffer, int length)
    {
        Sys.GetNonCryptographicallySecureRandomBytes(buffer, length);
    }
}
