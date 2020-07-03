// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Globalization
{
    internal static partial class GlobalizationMode
    {
        // Order of these properties in Windows matter because GetUseIcuMode is dependent on Invariant.
        // So we need Invariant to be initialized first.
        internal static bool Invariant { get; } = GetGlobalizationInvariantMode();

        internal static bool UseNls => false;

        private static bool GetGlobalizationInvariantMode()
        {
            throw new NotImplementedException();
        }

        private static void LoadAppLocalIcuCore(ReadOnlySpan<char> version, ReadOnlySpan<char> suffix)
        {
            throw new NotImplementedException();
        }
    }
}
