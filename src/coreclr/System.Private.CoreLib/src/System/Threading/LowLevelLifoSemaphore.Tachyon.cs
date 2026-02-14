// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Threading
{
    /// <summary>
    /// A LIFO semaphore implemented using the PAL's semaphore with uninterruptible waits.
    /// </summary>
    internal sealed partial class LowLevelLifoSemaphore : IDisposable
    {
        private void Create(int maximumSignalCount) => throw new NotImplementedException();

        public bool WaitCore(int timeoutMs) => throw new NotImplementedException();

        protected override void ReleaseCore(int count) => throw new NotImplementedException();

        public void Dispose() => throw new NotImplementedException();
    }
}
