// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System
{
    public static partial class Environment
    {
        //public static bool UserInteractive => true;

        private static string CurrentDirectoryCore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        private static string ExpandEnvironmentVariablesCore(string name)
        {
            throw new NotImplementedException();
        }

        private static bool Is64BitOperatingSystemWhen32BitProcess => throw new NotImplementedException();

        internal const string NewLineConst = "\n";

        private static int GetSystemPageSize() => throw new NotImplementedException();

        public static long WorkingSet
        {
            get
            {throw new NotImplementedException();
            }
        }

        public static string MachineName { get { throw new NotImplementedException(); } }

        public static string UserName => throw new NotImplementedException();

        public static string UserDomainName => throw new NotImplementedException();

        public static bool UserInteractive => throw new NotImplementedException();

        public static string SystemDirectory => throw new NotImplementedException();

        public static string[] GetLogicalDrives() => throw new NotImplementedException();

        private static OperatingSystem GetOSVersion() => throw new NotImplementedException();

        private static string? GetEnvironmentVariableFromRegistry(string variable, bool fromMachine) => null;

        private static void SetEnvironmentVariableFromRegistry(string variable, string? value, bool fromMachine) { }

        private static System.Collections.IDictionary GetEnvironmentVariablesFromRegistry(bool fromMachine) => throw new NotImplementedException();

        private static string GetFolderPathCore(SpecialFolder folder, SpecialFolderOption option)
        {
            throw new NotImplementedException();
        }

        private static int GetProcessId() => throw new NotImplementedException();

        private static string? GetProcessPath() => throw new NotImplementedException();

        private static string[] GetCommandLineArgsNative()
        {
            // This is only used for delegate created from native host

            // Consider to use /proc/self/cmdline to get command line
            return Array.Empty<string>();
        }
    }
}
