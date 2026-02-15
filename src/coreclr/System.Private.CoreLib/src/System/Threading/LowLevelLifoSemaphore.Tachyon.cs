// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Threading
{
    internal sealed partial class LowLevelLifoSemaphore : IDisposable
    {
        private void Create(int maximumSignalCount) => throw new NotImplementedException();

        private bool WaitCore(int timeoutMs) => throw new NotImplementedException();

        private void ReleaseCore(int count) => throw new NotImplementedException();

        public void Dispose() => throw new NotImplementedException();
    }
}
