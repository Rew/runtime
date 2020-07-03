// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        public static bool UserInteractive => true;

        private static string CurrentDirectoryCore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        private static string ExpandEnvironmentVariablesCore(string name)
        {
            throw new NotImplementedException();
        }

        public static string[] GetLogicalDrives() => throw new NotImplementedException();

        private static bool Is64BitOperatingSystemWhen32BitProcess => throw new NotImplementedException();

        public static string MachineName
        {
            get
            {throw new NotImplementedException();
            }
        }

        internal const string NewLineConst = "\n";

        public static string SystemDirectory => throw new NotImplementedException();

        public static int SystemPageSize => throw new NotImplementedException();

        public static unsafe string UserName
        {
            get
            {throw new NotImplementedException();

            }
        }

        private static unsafe bool TryGetUserNameFromPasswd(byte* buf, int bufLen, out string? username)
        {throw new NotImplementedException();
        }

        public static string UserDomainName => MachineName;

        public static long WorkingSet
        {
            get
            {throw new NotImplementedException();
            }
        }

        private static OperatingSystem GetOSVersion() => throw new NotImplementedException();

        private static string? GetEnvironmentVariableFromRegistry(string variable, bool fromMachine) => null;

        private static void SetEnvironmentVariableFromRegistry(string variable, string? value, bool fromMachine) { }

        private static System.Collections.IDictionary GetEnvironmentVariablesFromRegistry(bool fromMachine) => throw new NotImplementedException();

        private static string GetFolderPathCore(SpecialFolder folder, SpecialFolderOption option)
        {
            throw new NotImplementedException();
        }
    }
}