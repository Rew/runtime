// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Threading
{
    public abstract partial class WaitHandle
    {
        private static int WaitOneCore(IntPtr handle, int millisecondsTimeout, bool useTrivialWaits)
        {
            throw new NotImplementedException();
        }

        private static int WaitMultipleIgnoringSyncContextCore(ReadOnlySpan<IntPtr> handles, bool waitAll, int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        private static int SignalAndWaitCore(IntPtr handleToSignal, IntPtr handleToWaitOn, int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }
    }
}
