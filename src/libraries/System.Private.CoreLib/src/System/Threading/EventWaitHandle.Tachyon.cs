// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Threading
{
    public partial class EventWaitHandle
    {
        private void CreateEventCore(bool initialState, EventResetMode mode)
        {
            throw new NotImplementedException();
        }

        private void CreateEventCore(bool initialState, EventResetMode mode, string? name, NamedWaitHandleOptionsInternal options, out bool createdNew)
        {
            throw new NotImplementedException();
        }

        private static OpenExistingResult OpenExistingWorker(string name, NamedWaitHandleOptionsInternal options, out EventWaitHandle? result)
        {
            throw new NotImplementedException();
        }

        public bool Set()
        {
            throw new NotImplementedException();
        }

        public bool Reset()
        {
            throw new NotImplementedException();
        }

        internal static bool Set(Microsoft.Win32.SafeHandles.SafeWaitHandle waitHandle)
        {
            throw new NotImplementedException();
        }
    }
}
