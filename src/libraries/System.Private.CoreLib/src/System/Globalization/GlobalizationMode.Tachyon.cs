// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Globalization
{
    internal static partial class GlobalizationMode
    {
        internal static bool UseNls => false;

        private static void LoadAppLocalIcuCore(ReadOnlySpan<char> version, ReadOnlySpan<char> suffix) => throw new NotImplementedException();

        // Order of these properties in Windows matter because GetUseIcuMode is dependent on Invariant.
        // So we need Invariant to be initialized first.
        /*internal static bool Invariant { get; } = GetGlobalizationInvariantMode();


        private static bool GetGlobalizationInvariantMode()
        {
            throw new NotImplementedException();
        }*/
    }
}
