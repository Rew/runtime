// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Threading
{
    public sealed partial class Mutex
    {
        private void CreateMutexCore(bool initiallyOwned, string? name, out bool createdNew)
        {
            throw new NotImplementedException();
        }

        private static OpenExistingResult OpenExistingWorker(string name, out Mutex? result)
        {
            throw new NotImplementedException();
        }

        public void ReleaseMutex()
        {
            throw new NotImplementedException();
        }
    }
}
