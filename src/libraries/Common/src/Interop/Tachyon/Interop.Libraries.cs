// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

internal static partial class Interop
{
    internal static partial class Libraries
    {
        private const string OS_Pal = "PAL_interop.dll";

        internal const string HostPolicy = OS_Pal;
        internal const string Kernel32 = OS_Pal;
        internal const string Ole32 = OS_Pal;
        internal const string OleAut32 = OS_Pal;
        internal const string GlobalizationNative = "System.Globalization.Native";

        // Host services declared with the System.Native PAL's entry-point names; the
        // library name is not consulted (every non-wildcard import is an extern symbol).
        internal const string SystemNative = "libSystem.Native";
    }
}
