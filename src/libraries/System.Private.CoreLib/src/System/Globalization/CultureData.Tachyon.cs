// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        internal bool IsWin32Installed => false;

        internal static unsafe CultureData GetCurrentRegionData() =>            throw new NotImplementedException();
    }
}
