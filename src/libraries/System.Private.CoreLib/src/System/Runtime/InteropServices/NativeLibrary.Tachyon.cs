// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.InteropServices
{
    public static partial class NativeLibrary
    {
        private const int LoadWithAlteredSearchPathFlag = 0;

        private static IntPtr LoadLibraryHelper(string libraryName, int flags, ref LoadLibErrorTracker errorTracker)
        {
            throw new NotImplementedException();
        }

        private static void FreeLib(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        private static unsafe IntPtr GetSymbolOrNull(IntPtr handle, string symbolName)
        {
            throw new NotImplementedException();
        }

        internal struct LoadLibErrorTracker
        {
            private string? _errorMessage;

            public void TrackErrorMessage(string? message)
            {
                _errorMessage = message;
            }

            public void Throw(string libraryName) => throw new DllNotFoundException(_errorMessage ?? libraryName);
        }
    }
}
