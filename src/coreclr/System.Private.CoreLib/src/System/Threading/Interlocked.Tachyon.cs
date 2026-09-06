// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Threading
{
    /// <summary>
    /// Tachyon: Interlocked.CoreCLR.cs marks the primitive operations [Intrinsic] and
    /// gives them self-recursive bodies ("Must expand intrinsic") that only work when
    /// the JIT replaces the call with lock cmpxchg / xchg / xadd. Witschi compiles the
    /// IL as written, so those bodies recurse until the stack is gone. This file is the
    /// same surface without the intrinsic branch: every primitive is an InternalCall
    /// that ResolveCallTargets retargets to Tachyon.Runtime.Interlocked by name and
    /// parameter count (a ref parameter crosses as a pointer). The runtime decides how
    /// to make them atomic — a compiler intrinsic later, one thread today.
    /// </summary>
    public static partial class Interlocked
    {
        #region Increment
        public static int Increment(ref int location) =>
            Add(ref location, 1);

        public static long Increment(ref long location) =>
            Add(ref location, 1);
        #endregion

        #region Decrement
        public static int Decrement(ref int location) =>
            Add(ref location, -1);

        public static long Decrement(ref long location) =>
            Add(ref location, -1);
        #endregion

        #region Exchange
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Exchange(ref int location1, int value)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return Exchange32(ref location1, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int Exchange32(ref int location1, int value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Exchange(ref long location1, long value)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return Exchange64(ref location1, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern long Exchange64(ref long location1, long value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location1))]
        public static object? Exchange([NotNullIfNotNull(nameof(value))] ref object? location1, object? value)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return ExchangeObject(ref location1, value);
        }

        [return: NotNullIfNotNull(nameof(location1))]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern object? ExchangeObject([NotNullIfNotNull(nameof(value))] ref object? location1, object? value);
        #endregion

        #region CompareExchange
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareExchange(ref int location1, int value, int comparand)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return CompareExchange32(ref location1, value, comparand);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int CompareExchange32(ref int location1, int value, int comparand);

        // Pointer form, used where a managed ref to the location is unsafe.
        internal static unsafe int CompareExchange(int* location1, int value, int comparand)
        {
            Debug.Assert(location1 != null);
            return CompareExchange32Pointer(location1, value, comparand);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern unsafe int CompareExchange32Pointer(int* location1, int value, int comparand);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CompareExchange(ref long location1, long value, long comparand)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return CompareExchange64(ref location1, value, comparand);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern long CompareExchange64(ref long location1, long value, long comparand);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location1))]
        public static object? CompareExchange(ref object? location1, object? value, object? comparand)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return CompareExchangeObject(ref location1, value, comparand);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [return: NotNullIfNotNull(nameof(location1))]
        private static extern object? CompareExchangeObject(ref object? location1, object? value, object? comparand);
        #endregion

        #region Add
        public static int Add(ref int location1, int value) =>
            ExchangeAdd(ref location1, value) + value;

        public static long Add(ref long location1, long value) =>
            ExchangeAdd(ref location1, value) + value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ExchangeAdd(ref int location1, int value)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return ExchangeAdd32(ref location1, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int ExchangeAdd32(ref int location1, int value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long ExchangeAdd(ref long location1, long value)
        {
            if (Unsafe.IsNullRef(ref location1))
                ThrowHelper.ThrowNullReferenceException();
            return ExchangeAdd64(ref location1, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern long ExchangeAdd64(ref long location1, long value);
        #endregion

        #region Read
        public static long Read(ref readonly long location) =>
            CompareExchange(ref Unsafe.AsRef(in location), 0, 0);
        #endregion
    }
}
