// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Diagnostics
{
    public partial class Stopwatch
    {
        private static long GetFrequency() => Tachyon.Stopwatch.GetFrequency();

        public static long GetTimestamp() => Tachyon.Stopwatch.GetTimestamp();
    }
}
