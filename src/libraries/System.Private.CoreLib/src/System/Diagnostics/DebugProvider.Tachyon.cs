// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Diagnostics
{
    public partial class DebugProvider
    {
        public static void FailCore(string stackTrace, string? message, string? detailMessage, string errorSource)
        {
            throw new NotImplementedException();
        }

        public static void WriteCore(string message)
        {
            throw new NotImplementedException();
        }

        private static void WriteToDebugger(string message)
        {
            throw new NotImplementedException();
        }

        private static void WriteToStderr(string message)
        {
            throw new NotImplementedException();
        }
    }
}
