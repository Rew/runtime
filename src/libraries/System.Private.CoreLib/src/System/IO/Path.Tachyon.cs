// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.IO
{
    public static partial class Path
    {
        public static char[] GetInvalidFileNameChars() => new char[] { '\0', '/' };

        public static char[] GetInvalidPathChars() => new char[] { '\0' };

        // Checks if the given path is available for use.
        private static bool ExistsCore(string fullPath, out bool isDirectory)
        {
                throw new NotImplementedException();
        }

        // Expands the given path to a fully qualified path.
        public static string GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        public static string GetFullPath(string path, string basePath)
        {
            throw new NotImplementedException();
        }

        public static bool IsPathRooted(ReadOnlySpan<char> path)
        {
            throw new NotImplementedException();
        }

        public static bool IsPathRooted([NotNullWhen(true)] string? path)
        {
            throw new NotImplementedException();
        }

        public static string GetTempPath()
        {
            throw new NotImplementedException();
        }

        public static string GetTempFileName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the path root or null if path is empty or null.
        /// </summary>
        public static string? GetPathRoot(string? path)
        {
            throw new NotImplementedException();
        }

        public static ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path)
        {
            throw new NotImplementedException();
        }
    }
}
