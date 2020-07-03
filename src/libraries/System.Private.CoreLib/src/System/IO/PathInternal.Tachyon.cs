// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
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
        [return: NotNullIfNotNull("path")]
        internal static string? NormalizeDirectorySeparators(string? path)
        {
            throw new NotImplementedException();
        }

        internal static bool IsPartiallyQualified(ReadOnlySpan<char> path)
        {
            // This is much simpler than Windows where paths can be rooted, but not fully qualified (such as Drive Relative)
            // As long as the path is rooted in Unix it doesn't use the current directory and therefore is fully qualified.
            throw new NotImplementedException();
        }

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
