// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace System.Runtime.InteropServices
{
    public sealed partial class PosixSignalRegistration
    {
        private static PosixSignalRegistration Register(PosixSignal signal, Action<PosixSignalContext> handler)
        {
            throw new NotImplementedException();
        }

        private void Unregister()
        {
            throw new NotImplementedException();
        }
    }
}
