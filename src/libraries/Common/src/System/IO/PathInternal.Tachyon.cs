// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace System.IO
{
    /// <summary>Contains internal path helpers that are shared between many projects.</summary>
    internal static partial class PathInternal
    {
        internal const char DirectorySeparatorChar = '/';
        internal const char AltDirectorySeparatorChar = '/';
        internal const char VolumeSeparatorChar = '/';
        internal const char PathSeparator = ':';
        internal const string DirectorySeparatorCharAsString = "/";

        internal const string DirectorySeparators = DirectorySeparatorCharAsString;
        internal const string ParentDirectoryPrefix = @"../";

        internal static int GetRootLength(ReadOnlySpan<char> path)
        {
            throw new NotImplementedException();
        }

        internal static bool IsDirectorySeparator(char c)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Normalize separators in the given path. Compresses forward slash runs.
        /// </summary>
        [return: NotNullIfNotNull(nameof(path))]
        internal static string? NormalizeDirectorySeparators(string? path) => throw new NotImplementedException();

        internal static bool IsPartiallyQualified(ReadOnlySpan<char> path) => throw new NotImplementedException();

        /// <summary>
        /// Returns true if the path is effectively empty for the current OS.
        /// For unix, this is empty or null. For Windows, this is empty, null, or
        /// just spaces ((char)32).
        /// </summary>
        internal static bool IsEffectivelyEmpty(string? path)
        {
            throw new NotImplementedException();
        }

        internal static bool IsEffectivelyEmpty(ReadOnlySpan<char> path)
        {
            throw new NotImplementedException();
        }
    }
}
