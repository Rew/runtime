// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
    /// <summary>Provides an implementation of FileSystem for Unix systems.</summary>
    internal static partial class FileSystem
    {
        public static void CopyFile(string sourceFullPath, string destFullPath, bool overwrite) => throw new NotImplementedException();

        public static void Encrypt(string path) => throw new NotImplementedException();

        public static void Decrypt(string path) => throw new NotImplementedException();

        public static void ReplaceFile(string sourceFullPath, string destFullPath, string? destBackupFullPath, bool ignoreMetadataErrors) => throw new NotImplementedException();

        public static void MoveFile(string sourceFullPath, string destFullPath) => throw new NotImplementedException();

        public static void MoveFile(string sourceFullPath, string destFullPath, bool overwrite) => throw new NotImplementedException();

        public static void DeleteFile(string fullPath) => throw new NotImplementedException();

        public static void CreateDirectory(string fullPath) => throw new NotImplementedException();

        private static void MoveDirectory(string sourceFullPath, string destFullPath, bool isCaseSensitiveRename) => throw new NotImplementedException();

        public static void RemoveDirectory(string fullPath, bool recursive) => throw new NotImplementedException();

        private static void RemoveDirectoryRecursive(string fullPath) => throw new NotImplementedException();

        private static bool RemoveEmptyDirectory(string fullPath, bool topLevel = false, bool throwWhenNotEmpty = true) => throw new NotImplementedException();

        public static FileAttributes GetAttributes(string fullPath) => throw new NotImplementedException();

        public static FileAttributes GetAttributes(SafeFileHandle fileHandle) => throw new NotImplementedException();

        public static void SetAttributes(string fullPath, FileAttributes attributes) => throw new NotImplementedException();

        public static void SetAttributes(SafeFileHandle fileHandle, FileAttributes attributes) => throw new NotImplementedException();

        public static DateTimeOffset GetCreationTime(string fullPath) => throw new NotImplementedException();

        public static DateTimeOffset GetCreationTime(SafeFileHandle fileHandle) => throw new NotImplementedException();

        public static void SetCreationTime(string fullPath, DateTimeOffset time, bool asDirectory) => throw new NotImplementedException();
        public static void SetCreationTime(SafeFileHandle fileHandle, DateTimeOffset time) => throw new NotImplementedException();

        public static DateTimeOffset GetLastAccessTime(string fullPath) => throw new NotImplementedException();

        public static DateTimeOffset GetLastAccessTime(SafeFileHandle fileHandle) => throw new NotImplementedException();

        public static void SetLastAccessTime(string fullPath, DateTimeOffset time, bool asDirectory) => throw new NotImplementedException();

        public static void SetLastAccessTime(SafeFileHandle fileHandle, DateTimeOffset time) => throw new NotImplementedException();

        public static DateTimeOffset GetLastWriteTime(string fullPath) => throw new NotImplementedException();

        public static DateTimeOffset GetLastWriteTime(SafeFileHandle fileHandle) => throw new NotImplementedException();

        public static void SetLastWriteTime(string fullPath, DateTimeOffset time, bool asDirectory) => throw new NotImplementedException();

        public static void SetLastWriteTime(SafeFileHandle fileHandle, DateTimeOffset time) => throw new NotImplementedException();

        public static string[] GetLogicalDrives() => throw new NotImplementedException();

        internal static string? GetLinkTarget(ReadOnlySpan<char> linkPath, bool isDirectory) => throw new NotImplementedException();

        internal static void CreateSymbolicLink(string path, string pathToTarget, bool isDirectory) => throw new NotImplementedException();

        internal static FileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget, bool isDirectory) => throw new NotImplementedException();

        public static bool DirectoryExists(ReadOnlySpan<char> fullPath) => throw new NotImplementedException();

        public static bool FileExists(ReadOnlySpan<char> fullPath) => throw new NotImplementedException();
    }
}
