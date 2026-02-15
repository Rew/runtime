// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;

namespace System.Threading
{
    public sealed partial class Thread
    {
        // the closest analog to Sleep(0) on Unix is sched_yield
        internal static void UninterruptibleSleep0() => Thread.Yield();

        internal static int GetCurrentProcessorNumber() => throw new NotImplementedException();

        private static void SleepInternal(int millisecondsTimeout) => throw new NotImplementedException();

#if !MONO
        private bool JoinInternal(int millisecondsTimeout) => throw new NotImplementedException();
#endif
    }
}
