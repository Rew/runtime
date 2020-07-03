// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace System.Globalization
{
    public partial class CultureInfo : IFormatProvider
    {
        internal static CultureInfo GetUserDefaultCulture()
        {
            throw new NotImplementedException();
        }

        private static CultureInfo GetUserDefaultUICulture()
        {
            return InitializeUserDefaultCulture();
        }
    }
}
