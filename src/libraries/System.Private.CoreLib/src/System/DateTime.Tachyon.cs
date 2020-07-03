// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System
{
    public readonly partial struct DateTime
    {

        internal static bool SystemSupportsLeapSeconds => false;

        public static DateTime UtcNow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private static DateTime FromFileTimeLeapSecondsAware(ulong fileTime) => default;
        private static ulong ToFileTimeLeapSecondsAware(long ticks) => default;

        // IsValidTimeWithLeapSeconds is not expected to be called at all for now on non-Windows platforms
        internal static bool IsValidTimeWithLeapSeconds(int year, int month, int day, int hour, int minute, DateTimeKind kind) => false;
    }
}
