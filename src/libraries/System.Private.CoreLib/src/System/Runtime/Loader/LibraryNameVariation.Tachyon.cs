// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;

namespace System.Runtime.Loader
{
    internal partial struct LibraryNameVariation
    {
        internal static IEnumerable<LibraryNameVariation> DetermineLibraryNameVariations(string libName, bool isRelativePath, bool forOSLoader = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
