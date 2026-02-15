// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Threading
{
    public sealed partial class Semaphore
    {
        private void CreateSemaphoreCore(int initialCount, int maximumCount)
        {
            throw new NotImplementedException();
        }

        private void CreateSemaphoreCore(int initialCount, int maximumCount, string? name, NamedWaitHandleOptionsInternal options, out bool createdNew)
        {
            throw new NotImplementedException();
        }

        private static OpenExistingResult OpenExistingWorker(string name, NamedWaitHandleOptionsInternal options, out Semaphore? result)
        {
            throw new NotImplementedException();
        }

        private int ReleaseCore(int releaseCount)
        {
            throw new NotImplementedException();
        }
    }
}
