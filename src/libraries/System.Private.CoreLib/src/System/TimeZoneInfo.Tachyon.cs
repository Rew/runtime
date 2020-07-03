// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Security;

using Internal.IO;

namespace System
{
    public sealed partial class TimeZoneInfo
    {
        /*private const string DefaultTimeZoneDirectory = "/usr/share/zoneinfo/";
        private const string ZoneTabFileName = "zone.tab";
        private const string TimeZoneEnvironmentVariable = "TZ";
        private const string TimeZoneDirectoryEnvironmentVariable = "TZDIR";
        private const string FallbackCultureName = "en-US";*/

        private TimeZoneInfo(byte[] data, string id, bool dstDisabled)
        {
            throw new NotImplementedException(LocalId);
        }

        private unsafe void GetDisplayName(Interop.Globalization.TimeZoneDisplayNameType nameType, string uiCulture, ref string? displayName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a cloned array of AdjustmentRule objects
        /// </summary>
        public AdjustmentRule[] GetAdjustmentRules()
        {
            throw new NotImplementedException();
        }

        private static void PopulateAllSystemTimeZones(CachedData cachedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper function for retrieving the local system time zone.
        /// May throw COMException, TimeZoneNotFoundException, InvalidTimeZoneException.
        /// Assumes cachedData lock is taken.
        /// </summary>
        /// <returns>A new TimeZoneInfo instance.</returns>
        private static TimeZoneInfo GetLocalTimeZone(CachedData cachedData)
        {
            throw new NotImplementedException();
        }

        private static TimeZoneInfoResult TryGetTimeZoneFromLocalMachine(string id, out TimeZoneInfo? value, out Exception? e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a collection of TimeZone Id values from the zone.tab file in the timeZoneDirectory.
        /// </summary>
        /// <remarks>
        /// Lines that start with # are comments and are skipped.
        /// </remarks>
        private static List<string> GetTimeZoneIds(string timeZoneDirectory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the tzfile raw data for the current 'local' time zone using the following rules.
        /// 1. Read the TZ environment variable.  If it is set, use it.
        /// 2. Look for the data in /etc/localtime.
        /// 3. Look for the data in GetTimeZoneDirectory()/localtime.
        /// 4. Use UTC if all else fails.
        /// </summary>
        private static bool TryGetLocalTzFile([NotNullWhen(true)] out byte[]? rawData, [NotNullWhen(true)] out string? id)
        {
            throw new NotImplementedException();
        }

        private static string? GetTzEnvironmentVariable()
        {
            throw new NotImplementedException();
        }

        private static bool TryLoadTzFile(string tzFilePath, [NotNullWhen(true)] ref byte[]? rawData, [NotNullWhen(true)] ref string? id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the time zone id by using 'readlink' on the path to see if tzFilePath is
        /// a symlink to a file.
        /// </summary>
        private static string? FindTimeZoneIdUsingReadLink(string tzFilePath)
        {
            throw new NotImplementedException();
        }

        /*private static string? GetDirectoryEntryFullPath(ref Interop.Sys.DirectoryEntry dirent, string currentPath)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Enumerate files
        /// </summary>
        private static unsafe void EnumerateFilesRecursively(string path, Predicate<string> condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find the time zone id by searching all the tzfiles for the one that matches rawData
        /// and return its file name.
        /// </summary>
        private static string FindTimeZoneId(byte[] rawData)
        {
            throw new NotImplementedException();
        }

        private static bool CompareTimeZoneFile(string filePath, byte[] buffer, byte[] rawData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper function used by 'GetLocalTimeZone()' - this function wraps the call
        /// for loading time zone data from computers without Registry support.
        ///
        /// The TryGetLocalTzFile() call returns a Byte[] containing the compiled tzfile.
        /// </summary>
        private static TimeZoneInfo GetLocalTimeZoneFromTzFile()
        {
            throw new NotImplementedException();
        }

        private static TimeZoneInfo? GetTimeZoneFromTzData(byte[]? rawData, string id)
        {
            throw new NotImplementedException();
        }

        private static string GetTimeZoneDirectory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper function for retrieving a TimeZoneInfo object by time_zone_name.
        /// This function wraps the logic necessary to keep the private
        /// SystemTimeZones cache in working order
        ///
        /// This function will either return a valid TimeZoneInfo instance or
        /// it will throw 'InvalidTimeZoneException' / 'TimeZoneNotFoundException'.
        /// </summary>
        public static TimeZoneInfo FindSystemTimeZoneById(string id)
        {
            throw new NotImplementedException();
        }

        // DateTime.Now fast path that avoids allocating an historically accurate TimeZoneInfo.Local and just creates a 1-year (current year) accurate time zone
        internal static TimeSpan GetDateTimeNowUtcOffsetFromUtc(DateTime time, out bool isAmbiguousLocalDst)
        {
            throw new NotImplementedException();
        }

        // TZFILE(5)                   BSD File Formats Manual                  TZFILE(5)
        //
        // NAME
        //      tzfile -- timezone information
        //
        // SYNOPSIS
        //      #include "/usr/src/lib/libc/stdtime/tzfile.h"
        //
        // DESCRIPTION
        //      The time zone information files used by tzset(3) begin with the magic
        //      characters ``TZif'' to identify them as time zone information files, fol-
        //      lowed by sixteen bytes reserved for future use, followed by four four-
        //      byte values written in a ``standard'' byte order (the high-order byte of
        //      the value is written first).  These values are, in order:
        //
        //      tzh_ttisgmtcnt  The number of UTC/local indicators stored in the file.
        //      tzh_ttisstdcnt  The number of standard/wall indicators stored in the
        //                      file.
        //      tzh_leapcnt     The number of leap seconds for which data is stored in
        //                      the file.
        //      tzh_timecnt     The number of ``transition times'' for which data is
        //                      stored in the file.
        //      tzh_typecnt     The number of ``local time types'' for which data is
        //                      stored in the file (must not be zero).
        //      tzh_charcnt     The number of characters of ``time zone abbreviation
        //                      strings'' stored in the file.
        //
        //      The above header is followed by tzh_timecnt four-byte values of type
        //      long, sorted in ascending order.  These values are written in ``stan-
        //      dard'' byte order.  Each is used as a transition time (as returned by
        //      time(3)) at which the rules for computing local time change.  Next come
        //      tzh_timecnt one-byte values of type unsigned char; each one tells which
        //      of the different types of ``local time'' types described in the file is
        //      associated with the same-indexed transition time.  These values serve as
        //      indices into an array of ttinfo structures that appears next in the file;
        //      these structures are defined as follows:
        //
        //            struct ttinfo {
        //                    long    tt_gmtoff;
        //                    int     tt_isdst;
        //                    unsigned int    tt_abbrind;
        //            };
        //
        //      Each structure is written as a four-byte value for tt_gmtoff of type
        //      long, in a standard byte order, followed by a one-byte value for tt_isdst
        //      and a one-byte value for tt_abbrind.  In each structure, tt_gmtoff gives
        //      the number of seconds to be added to UTC, tt_isdst tells whether tm_isdst
        //      should be set by localtime(3) and tt_abbrind serves as an index into the
        //      array of time zone abbreviation characters that follow the ttinfo struc-
        //      ture(s) in the file.
        //
        //      Then there are tzh_leapcnt pairs of four-byte values, written in standard
        //      byte order; the first value of each pair gives the time (as returned by
        //      time(3)) at which a leap second occurs; the second gives the total number
        //      of leap seconds to be applied after the given time.  The pairs of values
        //      are sorted in ascending order by time.b
        //
        //      Then there are tzh_ttisstdcnt standard/wall indicators, each stored as a
        //      one-byte value; they tell whether the transition times associated with
        //      local time types were specified as standard time or wall clock time, and
        //      are used when a time zone file is used in handling POSIX-style time zone
        //      environment variables.
        //
        //      Finally there are tzh_ttisgmtcnt UTC/local indicators, each stored as a
        //      one-byte value; they tell whether the transition times associated with
        //      local time types were specified as UTC or local time, and are used when a
        //      time zone file is used in handling POSIX-style time zone environment
        //      variables.
        //
        //      localtime uses the first standard-time ttinfo structure in the file (or
        //      simply the first ttinfo structure in the absence of a standard-time
        //      structure) if either tzh_timecnt is zero or the time argument is less
        //      than the first transition time recorded in the file.
        //
        // SEE ALSO
        //      ctime(3), time2posix(3), zic(8)
        //
        // BSD                           September 13, 1994                           BSD
        //
        //
        //
        // TIME(3)                  BSD Library Functions Manual                  TIME(3)
        //
        // NAME
        //      time -- get time of day
        //
        // LIBRARY
        //      Standard C Library (libc, -lc)
        //
        // SYNOPSIS
        //      #include <time.h>
        //
        //      time_t
        //      time(time_t *tloc);
        //
        // DESCRIPTION
        //      The time() function returns the value of time in seconds since 0 hours, 0
        //      minutes, 0 seconds, January 1, 1970, Coordinated Universal Time, without
        //      including leap seconds.  If an error occurs, time() returns the value
        //      (time_t)-1.
        //
        //      The return value is also stored in *tloc, provided that tloc is non-null.
        //
        // ERRORS
        //      The time() function may fail for any of the reasons described in
        //      gettimeofday(2).
        //
        // SEE ALSO
        //      gettimeofday(2), ctime(3)
        //
        // STANDARDS
        //      The time function conforms to IEEE Std 1003.1-2001 (``POSIX.1'').
        //
        // BUGS
        //      Neither ISO/IEC 9899:1999 (``ISO C99'') nor IEEE Std 1003.1-2001
        //      (``POSIX.1'') requires time() to set errno on failure; thus, it is impos-
        //      sible for an application to distinguish the valid time value -1 (repre-
        //      senting the last UTC second of 1969) from the error return value.
        //
        //      Systems conforming to earlier versions of the C and POSIX standards
        //      (including older versions of FreeBSD) did not set *tloc in the error
        //      case.
        //
        // HISTORY
        //      A time() function appeared in Version 6 AT&T UNIX.
        //
        // BSD                              July 18, 2003                             BSD
        //
        //
        private static void TZif_GenerateAdjustmentRules(out AdjustmentRule[]? rules, TimeSpan baseUtcOffset, DateTime[] dts, byte[] typeOfLocalTime,
            TZifType[] transitionType, bool[] StandardTime, bool[] GmtTime, string? futureTransitionsPosixFormat)
        {
            throw new NotImplementedException();
        }

        private static void TZif_GenerateAdjustmentRule(ref int index, TimeSpan timeZoneBaseUtcOffset, List<AdjustmentRule> rulesList, DateTime[] dts,
            byte[] typeOfLocalTime, TZifType[] transitionTypes, bool[] StandardTime, bool[] GmtTime, string? futureTransitionsPosixFormat)
        {
            throw new NotImplementedException();
        }

        private static TimeSpan TZif_CalculateTransitionOffsetFromBase(TimeSpan transitionOffset, TimeSpan timeZoneBaseUtcOffset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the first standard-time transition type, or simply the first transition type
        /// if there are no standard transition types.
        /// </summary>>
        /// <remarks>
        /// from 'man tzfile':
        /// localtime(3)  uses the first standard-time ttinfo structure in the file
        /// (or simply the first ttinfo structure in the absence of a standard-time
        /// structure)  if  either tzh_timecnt is zero or the time argument is less
        /// than the first transition time recorded in the file.
        /// </remarks>
        private static TZifType TZif_GetEarlyDateTransitionType(TZifType[] transitionTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an AdjustmentRule given the POSIX TZ environment variable string.
        /// </summary>
        /// <remarks>
        /// See http://man7.org/linux/man-pages/man3/tzset.3.html for the format and semantics of this POSX string.
        /// </remarks>
        private static AdjustmentRule? TZif_CreateAdjustmentRuleForPosixFormat(string posixFormat, DateTime startTransitionDate, TimeSpan timeZoneBaseUtcOffset)
        {
            throw new NotImplementedException();
        }

        private static TimeSpan? TZif_ParseOffsetString(ReadOnlySpan<char> offset)
        {
            throw new NotImplementedException();
        }

        private static DateTime ParseTimeOfDay(ReadOnlySpan<char> time)
        {
            throw new NotImplementedException();
        }

        private static TransitionTime? TZif_CreateTransitionTimeFromPosixRule(ReadOnlySpan<char> date, ReadOnlySpan<char> time)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a string like Jn or n into month and day values.
        /// </summary>
        private static void TZif_ParseJulianDay(ReadOnlySpan<char> date, out int month, out int day)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a string like Mm.w.d into month, week and DayOfWeek values.
        /// </summary>
        /// <returns>
        /// true if the parsing succeeded; otherwise, false.
        /// </returns>
        private static bool TZif_ParseMDateRule(ReadOnlySpan<char> dateRule, out int month, out int week, out DayOfWeek dayOfWeek)
        {
            throw new NotImplementedException();
        }

        private static bool TZif_ParsePosixFormat(
            ReadOnlySpan<char> posixFormat,
            out ReadOnlySpan<char> standardName,
            out ReadOnlySpan<char> standardOffset,
            out ReadOnlySpan<char> daylightSavingsName,
            out ReadOnlySpan<char> daylightSavingsOffset,
            out ReadOnlySpan<char> start,
            out ReadOnlySpan<char> startTime,
            out ReadOnlySpan<char> end,
            out ReadOnlySpan<char> endTime)
        {
            throw new NotImplementedException();
        }

        private static ReadOnlySpan<char> TZif_ParsePosixName(ReadOnlySpan<char> posixFormat, ref int index)
        {
            throw new NotImplementedException();
        }

        private static ReadOnlySpan<char> TZif_ParsePosixOffset(ReadOnlySpan<char> posixFormat, ref int index) =>
            TZif_ParsePosixString(posixFormat, ref index, c => !char.IsDigit(c) && c != '+' && c != '-' && c != ':');

        private static void TZif_ParsePosixDateTime(ReadOnlySpan<char> posixFormat, ref int index, out ReadOnlySpan<char> date, out ReadOnlySpan<char> time)
        {
            throw new NotImplementedException();
        }

        private static ReadOnlySpan<char> TZif_ParsePosixDate(ReadOnlySpan<char> posixFormat, ref int index) =>
            TZif_ParsePosixString(posixFormat, ref index, c => c == '/' || c == ',');

        private static ReadOnlySpan<char> TZif_ParsePosixTime(ReadOnlySpan<char> posixFormat, ref int index) =>
            TZif_ParsePosixString(posixFormat, ref index, c => c == ',');

        private static ReadOnlySpan<char> TZif_ParsePosixString(ReadOnlySpan<char> posixFormat, ref int index, Func<char, bool> breakCondition)
        {
            throw new NotImplementedException();
        }

        // Returns the Substring from zoneAbbreviations starting at index and ending at '\0'
        // zoneAbbreviations is expected to be in the form: "PST\0PDT\0PWT\0\PPT"
        private static string TZif_GetZoneAbbreviation(string zoneAbbreviations, int index)
        {
            throw new NotImplementedException();
        }

        // Converts an array of bytes into an int - always using standard byte order (Big Endian)
        // per TZif file standard
        private static unsafe int TZif_ToInt32(byte[] value, int startIndex)
        {
            throw new NotImplementedException();
        }

        // Converts an array of bytes into a long - always using standard byte order (Big Endian)
        // per TZif file standard
        private static unsafe long TZif_ToInt64(byte[] value, int startIndex)
        {
            throw new NotImplementedException();
        }

        private static long TZif_ToUnixTime(byte[] value, int startIndex, TZVersion version) =>
            version != TZVersion.V1 ?
                TZif_ToInt64(value, startIndex) :
                TZif_ToInt32(value, startIndex);

        private static DateTime TZif_UnixTimeToDateTime(long unixTime) =>
            unixTime < DateTimeOffset.UnixMinSeconds ? DateTime.MinValue :
            unixTime > DateTimeOffset.UnixMaxSeconds ? DateTime.MaxValue :
            DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;

        private static void TZif_ParseRaw(byte[] data, out TZifHead t, out DateTime[] dts, out byte[] typeOfLocalTime, out TZifType[] transitionType,
                                          out string zoneAbbreviations, out bool[] StandardTime, out bool[] GmtTime, out string? futureTransitionsPosixFormat)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Normalize adjustment rule offset so that it is within valid range
        /// This method should not be called at all but is here in case something changes in the future
        /// or if really old time zones are present on the OS (no combination is known at the moment)
        /// </summary>
        private static void NormalizeAdjustmentRuleOffset(TimeSpan baseUtcOffset, [NotNull] ref AdjustmentRule adjustmentRule)
        {
            // Certain time zones such as:
            //       Time Zone  start date  end date    offset
            // -----------------------------------------------------
            // America/Yakutat  0001-01-01  1867-10-18   14:41:00
            // America/Yakutat  1867-10-18  1900-08-20   14:41:00
            // America/Sitka    0001-01-01  1867-10-18   14:58:00
            // America/Sitka    1867-10-18  1900-08-20   14:58:00
            // Asia/Manila      0001-01-01  1844-12-31  -15:56:00
            // Pacific/Guam     0001-01-01  1845-01-01  -14:21:00
            // Pacific/Saipan   0001-01-01  1845-01-01  -14:21:00
            //
            // have larger offset than currently supported by framework.
            // If for whatever reason we find that time zone exceeding max
            // offset of 14h this function will truncate it to the max valid offset.
            // Updating max offset may cause problems with interacting with SQL server
            // which uses SQL DATETIMEOFFSET field type which was originally designed to be
            // bit-for-bit compatible with DateTimeOffset.
            throw new NotImplementedException();
        }

        private struct TZifType
        {
            public const int Length = 6;

            public readonly TimeSpan UtcOffset;
            public readonly bool IsDst;
            public readonly byte AbbreviationIndex;

            public TZifType(byte[] data, int index)
            {
            throw new NotImplementedException();
            }
        }

        private struct TZifHead
        {
            public const int Length = 44;

            public readonly uint Magic; // TZ_MAGIC "TZif"
            public readonly TZVersion Version; // 1 byte for a \0 or 2 or 3
            // public byte[15] Reserved; // reserved for future use
            public readonly uint IsGmtCount; // number of transition time flags
            public readonly uint IsStdCount; // number of transition time flags
            public readonly uint LeapCount; // number of leap seconds
            public readonly uint TimeCount; // number of transition times
            public readonly uint TypeCount; // number of local time types
            public readonly uint CharCount; // number of abbreviated characters

            public TZifHead(byte[] data, int index)
            {
            throw new NotImplementedException();
            }
        }

        private enum TZVersion : byte
        {
            V1 = 0,
            V2,
            V3,
            // when adding more versions, ensure all the logic using TZVersion is still correct
        }
    }
}
