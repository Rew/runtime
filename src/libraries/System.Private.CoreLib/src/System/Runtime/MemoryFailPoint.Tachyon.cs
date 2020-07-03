// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Runtime
{
    public sealed partial class MemoryFailPoint
    {
        private static ulong GetTopOfMemory()
        {
            throw new NotImplementedException();
        }

        private static bool CheckForAvailableMemory(out ulong availPageFile, out ulong totalAddressSpaceFree)
        {
            throw new NotImplementedException();
        }

        // Based on the shouldThrow parameter, this will throw an exception, or
        // returns whether there is enough space.  In all cases, we update
        // our last known free address space, hopefully avoiding needing to
        // probe again.
        private static void CheckForFreeAddressSpace(ulong size, bool shouldThrow)
        {
            throw new NotImplementedException();
            // Unreachable until CheckForAvailableMemory is implemented
        }

        // Allocate a specified number of bytes, commit them and free them. This should enlarge
        // page file if necessary and possible.
        private static void GrowPageFileIfNecessaryAndPossible(UIntPtr numBytes)
        {
            throw new NotImplementedException();
            // Unreachable until CheckForAvailableMemory is implemented
        }
    }
}
