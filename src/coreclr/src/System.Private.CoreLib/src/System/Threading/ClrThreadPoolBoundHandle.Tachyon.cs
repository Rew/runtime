// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Threading
{
    public sealed partial class ThreadPoolBoundHandle
    {
        private static ThreadPoolBoundHandle BindHandleCore(SafeHandle handle)
        {
            Debug.Assert(handle != null);
            Debug.Assert(!handle.IsClosed);
            Debug.Assert(!handle.IsInvalid);

            throw new System.NotImplementedException();
        }
    }
}
