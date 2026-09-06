// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.Threading
{
    /// <summary>
    /// Tachyon: the object header word and the thin lock in it belong to the runtime
    /// (Tachyon.Runtime.ObjectHeader in the Witchcraft repo), not to CoreLib. This
    /// file is only the surface Monitor programs against; every operation is an
    /// InternalCall that ResolveCallTargets retargets to the Tachyon.Runtime method
    /// of the same name and parameter count. Keep names and arities in step with
    /// src/Tachyon.Runtime/ObjectHeader.cs; an object parameter crosses as a pointer.
    ///
    /// Nothing here knows where the header lives or what its bits mean, so the
    /// runtime can change either without a CoreLib rebuild.
    /// </summary>
    internal static class ObjectHeader
    {
        /// <summary>
        /// Takes the lock for the current thread, recursively if it already holds it.
        /// Returns only once the lock is held.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void AcquireThinLock(object obj);

        /// <summary>
        /// Takes the lock if it is free or already held by the current thread.
        /// Waits up to millisecondsTimeout for another owner to release it.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool TryAcquireThinLock(object obj, int millisecondsTimeout);

        public static bool TryAcquireThinLock(object obj) => TryAcquireThinLock(obj, 0);

        /// <summary>
        /// Releases one level of the current thread's hold. Not holding the lock is a
        /// runtime fatal (CoreCLR throws SynchronizationLockException here).
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void Release(object obj);

        /// <summary>True when the current thread holds the lock.</summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsAcquired(object obj);

        /// <summary>
        /// Monitor.Wait/Pulse need a Lock object per monitor; Tachyon has none yet.
        /// Declared without a runtime implementation on purpose: reaching it is a named
        /// fatal ("no Tachyon.Runtime implementation for ObjectHeader.GetLockObject")
        /// rather than a wrong answer.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Lock GetLockObject(object obj);
    }
}
