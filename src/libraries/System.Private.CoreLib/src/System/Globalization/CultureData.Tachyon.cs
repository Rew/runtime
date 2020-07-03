// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace System.Globalization
{
    internal partial class CultureData
    {
        /// <summary>
        /// This method uses the sRealName field (which is initialized by the constructor before this is called) to
        /// initialize the rest of the state of CultureData based on the underlying OS globalization library.
        /// </summary>
        private bool InitCultureDataCore()
        {
            throw new NotImplementedException();
        }

        private void InitUserOverride(bool useUserOverride)
        {
            throw new NotImplementedException();
        }

        private static string? LCIDToLocaleName(int culture)
        {
            throw new NotImplementedException();
        }

        internal static bool IsWin32Installed => false;
        private static bool ShouldUseUserOverrideNlsData => false;

        internal static unsafe CultureData GetCurrentRegionData() => throw new NotImplementedException();

        private string[]? GetTimeFormatsCore(bool shortFormat) { throw new NotImplementedException(); }
        private static int GetAnsiCodePage(string cultureName) { throw new NotImplementedException(); }
        private static int GetOemCodePage(string cultureName) { throw new NotImplementedException(); }
        private static int GetMacCodePage(string cultureName) { throw new NotImplementedException(); }
        private static int GetEbcdicCodePage(string cultureName) { throw new NotImplementedException(); }
    }
}
