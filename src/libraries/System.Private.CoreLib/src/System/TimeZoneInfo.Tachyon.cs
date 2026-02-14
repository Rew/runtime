// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Security;

namespace System
{
    public sealed partial class TimeZoneInfo
    {
        private static string? GetAlternativeId(string id, out bool idIsIana) => throw new NotImplementedException(LocalId);


        /// <summary>
        /// Returns a cloned array of AdjustmentRule objects
        /// </summary>
        public AdjustmentRule[] GetAdjustmentRules()
        {
            throw new NotImplementedException();
        }

        private static TimeZoneInfo GetLocalTimeZone(CachedData cachedData) => throw new NotImplementedException();

        private string? PopulateDisplayName()
        {
            throw new NotImplementedException();
        }

        private string? PopulateStandardDisplayName()
        {
            throw new NotImplementedException();
        }

        private string? PopulateDaylightDisplayName()
        {
            throw new NotImplementedException();
        }

        private static void PopulateAllSystemTimeZones(CachedData cachedData)
        {
            throw new NotImplementedException();
        }


        private static TimeZoneInfoResult TryGetTimeZoneFromLocalMachine(string id, out TimeZoneInfo? value, out Exception? e)
        {
            throw new NotImplementedException();
        }

        // Helper function to get the standard display name for the UTC static time zone instance
        private static string GetUtcStandardDisplayName()
        {
            throw new NotImplementedException();
        }

        // Helper function to get the full display name for the UTC static time zone instance
        private static string GetUtcFullDisplayName(string timeZoneId, string standardDisplayName)
        {
            throw new NotImplementedException();
        }

        private static TimeZoneInfoResult TryGetTimeZone(string id, out TimeZoneInfo? timeZone, out Exception? e, CachedData cachedData)
            => TryGetTimeZone(id, false, out timeZone, out e, cachedData, alwaysFallbackToLocalMachine: true);

        // DateTime.Now fast path that avoids allocating an historically accurate TimeZoneInfo.Local and just creates a 1-year (current year) accurate time zone
        internal static TimeSpan GetDateTimeNowUtcOffsetFromUtc(DateTime time, out bool isAmbiguousLocalDst)
        {
            throw new NotImplementedException();
        }

    }
}
