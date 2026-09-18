// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Tachyon: replaces RuntimeHelpers.CoreCLR.cs (selected by TargetsTachyon in
// System.Private.CoreLib.csproj); the CoreCLR file is never edited for Tachyon.
//
// These are the VM's half of the runtime, and Tachyon's runtime lives in the
// Witchcraft repo (Tachyon.Runtime), so the rule for this file is: a method
// that touches the VM — MethodTable fields, allocation, hashing, class
// construction — is a [MethodImpl(InternalCall)] extern here, implemented in
// Tachyon.Runtime.RuntimeHelpers under the same name and parameter count.
// Members are converted as startup reaches them (so far: RunClassConstructor).
// MethodTable reads no field of its own: every member is a TypeHandle_Get*
// service the compiler provides (see the struct), so the CoreLib code that
// compiles against it keeps working over Tachyon's metadata. TypeHandle stays
// as it is. Remaining QCalls are reached by nothing yet.

using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Threading;

namespace System.Runtime.CompilerServices
{
    public static partial class RuntimeHelpers
    {
        [Intrinsic]
        public static unsafe void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
        {
            if (array is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            if (fldHandle.IsNullHandle())
                throw new ArgumentException(SR.Argument_InvalidHandle);

            IRuntimeFieldInfo fldInfo = fldHandle.GetRuntimeFieldInfo();

            if (!RuntimeFieldHandle.GetRVAFieldInfo(fldInfo.Value, out void* address, out uint size))
                throw new ArgumentException(SR.Argument_BadFieldForInitializeArray);

            // Note that we do not check that the field is actually in the PE file that is initializing
            // the array. Basically, the data being published can be accessed by anyone with the proper
            // permissions (C# marks these as assembly visibility, and thus are protected from outside
            // snooping)

            MethodTable* pMT = GetMethodTable(array);
            TypeHandle elementTH = pMT->GetArrayElementTypeHandle();

            if (elementTH.IsTypeDesc || !elementTH.AsMethodTable()->IsPrimitive) // Enum is included
                throw new ArgumentException(SR.Argument_BadArrayForInitializeArray);

            nuint totalSize = pMT->ComponentSize * array.NativeLength;

            // make certain you don't go off the end of the rva static
            if (totalSize > size)
                throw new ArgumentException(SR.Argument_BadFieldForInitializeArray);

            ref byte src = ref *(byte*)address; // Ref is extending the lifetime of the static field.
            GC.KeepAlive(fldInfo);

            ref byte dst = ref MemoryMarshal.GetArrayDataReference(array);

            Debug.Assert(!elementTH.AsMethodTable()->ContainsGCPointers);

            if (BitConverter.IsLittleEndian)
            {
                SpanHelpers.Memmove(ref dst, ref src, totalSize);
            }
            else
            {
                switch (pMT->ComponentSize)
                {
                    case sizeof(byte):
                        SpanHelpers.Memmove(ref dst, ref src, totalSize);
                        break;
                    case sizeof(ushort):
                        BinaryPrimitives.ReverseEndianness(
                            new ReadOnlySpan<ushort>(ref Unsafe.As<byte, ushort>(ref src), array.Length),
                            new Span<ushort>(ref Unsafe.As<byte, ushort>(ref dst), array.Length));
                        break;
                    case sizeof(uint):
                        BinaryPrimitives.ReverseEndianness(
                            new ReadOnlySpan<uint>(ref Unsafe.As<byte, uint>(ref src), array.Length),
                            new Span<uint>(ref Unsafe.As<byte, uint>(ref dst), array.Length));
                        break;
                    case sizeof(ulong):
                        BinaryPrimitives.ReverseEndianness(
                            new ReadOnlySpan<ulong>(ref Unsafe.As<byte, ulong>(ref src), array.Length),
                            new Span<ulong>(ref Unsafe.As<byte, ulong>(ref dst), array.Length));
                        break;
                    default:
                        Debug.Fail("Incorrect primitive type size!");
                        break;
                }
            }
        }

        private static unsafe ref byte GetSpanDataFrom(
            RuntimeFieldHandle fldHandle,
            RuntimeTypeHandle targetTypeHandle,
            out int count)
        {
            if (fldHandle.IsNullHandle())
                throw new ArgumentException(SR.Argument_InvalidHandle);

            IRuntimeFieldInfo fldInfo = fldHandle.GetRuntimeFieldInfo();

            if (!RuntimeFieldHandle.GetRVAFieldInfo(fldInfo.Value, out void* data, out uint totalSize))
                throw new ArgumentException(SR.Argument_BadFieldForInitializeArray);

            TypeHandle th = targetTypeHandle.GetRuntimeType().GetNativeTypeHandle();
            Debug.Assert(!th.IsTypeDesc); // TypeDesc can't be used as generic parameter
            MethodTable* targetMT = th.AsMethodTable();

            if (!targetMT->IsPrimitive) // Enum is included
                throw new ArgumentException(SR.Argument_BadArrayForInitializeArray);

            uint targetTypeSize = targetMT->GetNumInstanceFieldBytes();
            Debug.Assert(uint.IsPow2(targetTypeSize));

            if (((nuint)data & (targetTypeSize - 1)) != 0)
                throw new ArgumentException(SR.Argument_BadFieldForInitializeArray);

            if (!BitConverter.IsLittleEndian)
            {
                throw new PlatformNotSupportedException();
            }

            count = (int)(totalSize / targetTypeSize);
            ref byte dataRef = ref *(byte*)data; // Ref is extending the lifetime of the static field.
            GC.KeepAlive(fldInfo);

            return ref dataRef;
        }

        // GetObjectValue is intended to allow value classes to be manipulated as 'Object'
        // but have aliasing behavior of a value class.  The intent is that you would use
        // this function just before an assignment to a variable of type 'Object'.  If the
        // value being assigned is a mutable value class, then a shallow copy is returned
        // (because value classes have copy semantics), but otherwise the object itself
        // is returned.
        //
        // Note: VB calls this method when they're about to assign to an Object
        // or pass it as a parameter.  The goal is to make sure that boxed
        // value types work identical to unboxed value types - ie, they get
        // cloned when you pass them around, and are always passed by value.
        // Of course, reference types are not cloned.
        //
        [return: NotNullIfNotNull(nameof(obj))]
        public static unsafe object? GetObjectValue(object? obj)
        {
            if (obj == null)
                return null;

            MethodTable* pMT = GetMethodTable(obj);

            if (!pMT->IsValueType || pMT->IsPrimitive)
                return obj;

            // Technically we could return boxed DateTimes and Decimals without
            // copying them here, but VB realized that this would be a breaking change
            // for their customers.  So copy them.

            return obj.MemberwiseClone();
        }

        // Tachyon: runtime internals are implemented in Tachyon.Runtime (RuntimeHelpers
        // there, matched by name and parameter count), not here. CoreCLR's version is a
        // QCall into the VM; Tachyon's runs every static constructor at startup before
        // any user code, so the implementation has nothing left to trigger.
        [RequiresUnreferencedCode("Trimmer can't guarantee existence of class constructor")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void RunClassConstructor(RuntimeTypeHandle type);

        // RunModuleConstructor causes the module constructor for the given type to be triggered
        // in the current domain.  After this call returns, the module constructor is guaranteed to
        // have at least been started by some thread.  In the absence of module constructor
        // deadlock conditions, the call is further guaranteed to have completed.
        //
        // This call will generate an exception if the specified module constructor threw an
        // exception when it ran.

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "ReflectionInvocation_RunModuleConstructor")]
        private static partial void RunModuleConstructor(QCallModule module);

        public static void RunModuleConstructor(ModuleHandle module)
        {
            RuntimeModule rm = module.GetRuntimeModule() ??
                throw new ArgumentException(SR.InvalidOperation_HandleIsNotInitialized, nameof(module));

            RunModuleConstructor(new QCallModule(ref rm));
        }

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "ReflectionInvocation_CompileMethod")]
        internal static partial void CompileMethod(RuntimeMethodHandleInternal method);

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "ReflectionInvocation_PrepareMethod")]
        private static unsafe partial void PrepareMethod(RuntimeMethodHandleInternal method, IntPtr* pInstantiation, int cInstantiation);

        public static void PrepareMethod(RuntimeMethodHandle method) => PrepareMethod(method, null);

        public static unsafe void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[]? instantiation)
        {
            IRuntimeMethodInfo methodInfo = method.GetMethodInfo() ??
                throw new ArgumentException(SR.InvalidOperation_HandleIsNotInitialized, nameof(method));

            // defensive copy of user-provided array, per CopyRuntimeTypeHandles contract
            instantiation = (RuntimeTypeHandle[]?)instantiation?.Clone();

            ReadOnlySpan<IntPtr> instantiationHandles = RuntimeTypeHandle.CopyRuntimeTypeHandles(instantiation, stackScratch: stackalloc IntPtr[8]);
            fixed (IntPtr* pInstantiation = instantiationHandles)
            {
                PrepareMethod(IRuntimeMethodInfo.GetValue(methodInfo), pInstantiation, instantiationHandles.Length);
                GC.KeepAlive(instantiation);
                GC.KeepAlive(methodInfo);
            }
        }

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "ReflectionInvocation_PrepareDelegate")]
        private static partial void PrepareDelegate(ObjectHandleOnStack d);

        public static void PrepareDelegate(Delegate d)
        {
            if (d is null)
            {
                return;
            }

            PrepareDelegate(ObjectHandleOnStack.Create(ref d));
        }

        /// <summary>
        /// If a hash code has been assigned to the object, it is returned. Otherwise zero is
        /// returned.
        /// </summary>
        /// <safety>Runtime FCall that reads the object's existing hash from its header and returns it as an int; it dereferences no raw pointer and touches no caller-chosen memory.</safety>
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern safe int TryGetHashCode(object? o);

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "ObjectNative_GetHashCodeSlow")]
        private static partial int GetHashCodeSlow(ObjectHandleOnStack o);

        public static int GetHashCode(object? o)
        {
            int hashCode = TryGetHashCode(o);
            if (hashCode == 0)
            {
                return GetHashCodeWorker(o);
            }
            return hashCode;

            [MethodImpl(MethodImplOptions.NoInlining)]
            static int GetHashCodeWorker(object? o)
            {
                if (o is null)
                {
                    return 0;
                }
                return GetHashCodeSlow(ObjectHandleOnStack.Create(ref o));
            }
        }

        public static new unsafe bool Equals(object? o1, object? o2)
        {
            // Compare by ref for normal classes, by value for value types.

            if (ReferenceEquals(o1, o2))
                return true;

            if (o1 is null || o2 is null)
                return false;

            MethodTable* pMT = GetMethodTable(o1);

            // If it's not a value class, don't compare by value
            if (!pMT->IsValueType)
                return false;

            // Make sure they are the same type.
            if (pMT != GetMethodTable(o2))
                return false;

            // Compare the contents
            return ContentEquals(o1, o2);
        }

        /// <safety>Runtime FCall that compares the field contents of two same-typed managed value objects and returns a bool; it works through type-checked object references and dereferences no raw pointer.</safety>
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern safe bool ContentEquals(object o1, object o2);

        [Obsolete("OffsetToStringData has been deprecated. Use string.GetPinnableReference() instead.")]
        public static int OffsetToStringData
        {
            // This offset is baked in by string indexer intrinsic, so there is no harm
            // in getting it baked in here as well.
            [NonVersionable]
            get =>
                // Number of bytes from the address pointed to by a reference to
                // a String to the first 16-bit character in the String.  Skip
                // over the MethodTable pointer, & String
                // length.  Of course, the String reference points to the memory
                // after the sync block, so don't count that.
                // This property allows C#'s fixed statement to work on Strings.
                // On 64 bit platforms, this should be 12 (8+4) and on 32 bit 8 (4+4).
#if TARGET_64BIT
                12;
#else // 32
                8;
#endif // TARGET_64BIT

        }

        // This method ensures that there is sufficient stack to execute the average Framework function.
        // If there is not enough stack, then it throws System.InsufficientExecutionStackException.
        // Note: this method is not to be confused with ProbeForSufficientStack.
        public static void EnsureSufficientExecutionStack()
        {
            if (!TryEnsureSufficientExecutionStack())
            {
                throw new InsufficientExecutionStackException();
            }
        }

        // This method ensures that there is sufficient stack to execute the average Framework function.
        // If there is not enough stack, then it return false.
        // Note: this method is not to be confused with ProbeForSufficientStack.
        /// <safety>Runtime FCall that only checks the current thread's remaining stack space; it takes no arguments and dereferences no caller-supplied memory.</safety>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern safe bool TryEnsureSufficientExecutionStack();

        public static object GetUninitializedObject(
            // This API doesn't call any constructors, but the type needs to be seen as constructed.
            // A type is seen as constructed if a constructor is kept.
            // This obviously won't cover a type with no constructor. Reference types with no
            // constructor are an academic problem. Valuetypes with no constructors are a problem,
            // but IL Linker currently treats them as always implicitly boxed.
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
            Type type)
        {
            if (type is not RuntimeType rt)
            {
                ArgumentNullException.ThrowIfNull(type);
                throw new SerializationException(SR.Format(SR.Serialization_InvalidType, type));
            }

            return rt.GetUninitializedObject();
        }

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "ObjectNative_AllocateUninitializedClone")]
        internal static partial void AllocateUninitializedClone(ObjectHandleOnStack objHandle);

        /// <returns>true if given type is bitwise equatable (memcmp can be used for equality checking)</returns>
        /// <remarks>
        /// Only use the result of this for Equals() comparison, not for CompareTo() comparison.
        /// </remarks>
        [Intrinsic]
        internal static bool IsBitwiseEquatable<T>()
        {
            // The body of this function will be replaced by the EE.
            // See getILIntrinsicImplementationForRuntimeHelpers for how this happens.
            throw new InvalidOperationException();
        }

        [Intrinsic]
        internal static bool EnumEquals<T>(T x, T y) where T : struct, Enum
        {
            // The body of this function will be replaced by the EE.
            // See getILIntrinsicImplementationForRuntimeHelpers for how this happens.
            return x.Equals(y);
        }

        [Intrinsic]
        internal static int EnumCompareTo<T>(T x, T y) where T : struct, Enum
        {
            // The body of this function will be replaced by the EE.
            // See getILIntrinsicImplementationForRuntimeHelpers for how this happens.
            return x.CompareTo(y);
        }

#if FEATURE_IJW
        [Intrinsic]
        internal static unsafe void CopyConstruct<T>(T* dest, T* src) where T : unmanaged
        {
            // The body of this function will be replaced by the EE.
            // See getILIntrinsicImplementationForRuntimeHelpers for how this happens.
            throw new InvalidOperationException();
        }
#endif

        [DebuggerHidden]
        [DebuggerStepThrough]
        internal static ref byte GetRawData(this object obj) =>
            ref Unsafe.As<RawData>(obj).Data;

        [DebuggerHidden]
        [DebuggerStepThrough]
        internal static ref nint GetMethodTableRef(this object obj)
            => ref Unsafe.Subtract(ref Unsafe.As<byte, nint>(ref GetRawData(obj)), 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe nuint GetRawObjectDataSize(object obj)
        {
            MethodTable* pMT = GetMethodTable(obj);

            // See comment on RawArrayData for details
            nuint rawSize = pMT->BaseSize - (nuint)(2 * sizeof(IntPtr));
            if (pMT->HasComponentSize)
                rawSize += (uint)Unsafe.As<RawArrayData>(obj).Length * (nuint)pMT->ComponentSize;

            GC.KeepAlive(obj); // Keep MethodTable alive

            return rawSize;
        }

        // Returns array element size.
        // Callers are required to keep obj alive
        internal static unsafe ushort GetElementSize(this Array array)
        {
            Debug.Assert(ObjectHasComponentSize(array));
            return GetMethodTable(array)->ComponentSize;
        }

        // Returns pointer to the multi-dimensional array bounds.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref int GetMultiDimensionalArrayBounds(this Array array)
        {
            Debug.Assert(GetMultiDimensionalArrayRank(array) > 0);
            // See comment on RawArrayData for details
            return ref Unsafe.As<byte, int>(ref Unsafe.As<RawArrayData>(array).Data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int GetMultiDimensionalArrayRank(this Array array)
        {
            int rank = GetMethodTable(array)->MultiDimensionalArrayRank;
            GC.KeepAlive(array); // Keep MethodTable alive
            return rank;
        }

        // Returns true iff the object has a component size;
        // i.e., is variable length like System.String or Array.
        // Callers are required to keep obj alive
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe bool ObjectHasComponentSize(object obj)
        {
            return GetMethodTable(obj)->HasComponentSize;
        }

        // Returns true iff the type of the object requires finalization,
        // which includes a finalizer inherited from a base type.
        internal static unsafe bool ObjectHasFinalizer(object obj)
        {
            bool hasFinalizer = GetMethodTable(obj)->HasFinalizer;
            GC.KeepAlive(obj); // Keep MethodTable alive
            return hasFinalizer;
        }

        /// <summary>
        /// Boxes a given value using an input <see cref="MethodTable"/> to determine its type.
        /// </summary>
        /// <param name="methodTable">The <see cref="MethodTable"/> pointer to use to create the boxed instance.</param>
        /// <param name="data">A reference to the data to box.</param>
        /// <returns>A boxed instance of the value at <paramref name="data"/>.</returns>
        /// <remarks>This method includes proper handling for nullable value types as well.</remarks>
        internal static unsafe object? Box(MethodTable* methodTable, ref byte data) =>
            methodTable->IsNullable ? CastHelpers.Box_Nullable(methodTable, ref data) : CastHelpers.Box(methodTable, ref data);

        // Given an object reference, returns its MethodTable*.
        //
        // WARNING: The caller has to ensure that MethodTable* does not get unloaded. The most robust way
        // to achieve this is by using GC.KeepAlive on the object that the MethodTable* was fetched from, e.g.:
        //
        // MethodTable* pMT = GetMethodTable(o);
        //
        // ... work with pMT ...
        //
        // GC.KeepAlive(o);
        //
        [Intrinsic]
        internal static unsafe MethodTable* GetMethodTable(object obj) => GetMethodTable(obj);

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "MethodTable_AreTypesEquivalent")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static unsafe partial bool AreTypesEquivalent(MethodTable* pMTa, MethodTable* pMTb);

        /// <summary>Allocates memory that's associated with the <paramref name="type" /> and is freed if and when the <see cref="Type" /> is unloaded.</summary>
        /// <param name="type">The type associated with the allocated memory.</param>
        /// <param name="size">The amount of memory to allocate, in bytes.</param>
        /// <returns>The allocated memory.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> must be a type provided by the runtime.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is negative.</exception>
        public static IntPtr AllocateTypeAssociatedMemory(Type type, int size)
        {
            if (type is not RuntimeType rt)
            {
                throw new ArgumentException(SR.Arg_MustBeType, nameof(type));
            }

            ArgumentOutOfRangeException.ThrowIfNegative(size);

            return AllocateTypeAssociatedMemory(new QCallTypeHandle(ref rt), (uint)size);
        }

        /// <summary>Allocates aligned memory that's associated with the <paramref name="type" /> and is freed if and when the <see cref="Type" /> is unloaded.</summary>
        /// <param name="type">The type associated with the allocated memory.</param>
        /// <param name="size">The amount of memory to allocate, in bytes.</param>
        /// <param name="alignment">The alignment, in bytes, of the memory to allocate. This must be a power of <c>2</c>.</param>
        /// <returns>The allocated aligned memory.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> must be a type provided by the runtime.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is negative.</exception>
        /// <exception cref="ArgumentException"><paramref name="alignment" /> is not a power of <c>2</c>.</exception>
        public static IntPtr AllocateTypeAssociatedMemory(Type type, int size, int alignment)
        {
            if (type is not RuntimeType rt)
            {
                throw new ArgumentException(SR.Arg_MustBeType, nameof(type));
            }

            ArgumentOutOfRangeException.ThrowIfNegative(size);

            if (!BitOperations.IsPow2(alignment))
            {
                // The C standard doesn't define what a valid alignment is, however Windows and POSIX implementation requires a power of 2
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlignmentMustBePow2);
            }

            return AllocateTypeAssociatedMemoryAligned(new QCallTypeHandle(ref rt), (uint)size, (uint)alignment);
        }

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "RuntimeTypeHandle_AllocateTypeAssociatedMemory")]
        private static partial IntPtr AllocateTypeAssociatedMemory(QCallTypeHandle type, uint size);

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "RuntimeTypeHandle_AllocateTypeAssociatedMemoryAligned")]
        private static partial IntPtr AllocateTypeAssociatedMemoryAligned(QCallTypeHandle type, uint size, uint alignment);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern unsafe TailCallArgBuffer* GetTailCallArgBuffer();

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(QCall, EntryPoint = "TailCallHelp_AllocTailCallArgBufferInternal")]
        private static unsafe partial TailCallArgBuffer* AllocTailCallArgBufferInternal(int size);

        private const int TAILCALLARGBUFFER_ACTIVE = 0;
        // private const int TAILCALLARGBUFFER_INSTARG_ONLY = 1;
        private const int TAILCALLARGBUFFER_INACTIVE = 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // To allow unrolling of Span.Clear
        private static unsafe TailCallArgBuffer* AllocTailCallArgBuffer(int size, IntPtr gcDesc)
        {
            TailCallArgBuffer* buffer = GetTailCallArgBuffer();
            if (buffer != null && buffer->Size >= size)
            {
                buffer->State = TAILCALLARGBUFFER_INACTIVE;
            }
            else
            {
                buffer = AllocTailCallArgBufferWorker(size);

                [MethodImpl(MethodImplOptions.NoInlining)]
                static TailCallArgBuffer* AllocTailCallArgBufferWorker(int size) => AllocTailCallArgBufferInternal(size);
            }
            Debug.Assert(buffer != null);
            Debug.Assert(buffer->Size >= size);
            Debug.Assert(buffer->State == TAILCALLARGBUFFER_INACTIVE);

            buffer->GCDesc = gcDesc;

            new Span<byte>(buffer + 1, size - sizeof(TailCallArgBuffer)).Clear();

            // The buffer is now ready to be used.
            buffer->State = TAILCALLARGBUFFER_ACTIVE;

            return buffer;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern unsafe TailCallTls* GetTailCallInfo(IntPtr retAddrSlot, IntPtr* retAddr);

        [StackTraceHidden]
        private static unsafe void DispatchTailCalls(
            IntPtr callersRetAddrSlot,
            delegate*<TailCallArgBuffer*, ref byte, PortableTailCallFrame*, void> callTarget,
            ref byte retVal)
        {
            IntPtr callersRetAddr;
            TailCallTls* tls = GetTailCallInfo(callersRetAddrSlot, &callersRetAddr);
            PortableTailCallFrame* prevFrame = tls->Frame;
            if (callersRetAddr == prevFrame->TailCallAwareReturnAddress)
            {
                prevFrame->NextCall = callTarget;
                return;
            }

            PortableTailCallFrame newFrame;
            // GC uses NextCall to keep LoaderAllocator alive after we link it below,
            // so we must null it out before that.
            newFrame.NextCall = null;

            try
            {
                tls->Frame = &newFrame;

                do
                {
                    callTarget(tls->ArgBuffer, ref retVal, &newFrame);
                    callTarget = newFrame.NextCall;
                } while (callTarget != null);
            }
            finally
            {
                tls->Frame = prevFrame;

                // If the arg buffer is reporting inst argument (TAILCALLARGBUFFER_INSTARG_ONLY), it is safe to abandon it now.
                tls->ArgBuffer->State = TAILCALLARGBUFFER_INACTIVE;
            }
        }

        /// <summary>
        /// Create a boxed object of the specified type from the data located at the target reference.
        /// </summary>
        /// <param name="target">The target data</param>
        /// <param name="type">The type of box to create.</param>
        /// <returns>A boxed object containing the specified data.</returns>
        /// <exception cref="ArgumentNullException">The specified type handle is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The specified type cannot have a boxed instance of itself created.</exception>
        /// <exception cref="NotSupportedException">The passed in type is a by-ref-like type.</exception>
        public static object? Box(ref byte target, RuntimeTypeHandle type)
        {
            if (type.IsNullHandle())
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.type);

            return type.GetRuntimeType().Box(ref target);
        }

        [LibraryImport(QCall, EntryPoint = "ReflectionInvocation_SizeOf")]
        [SuppressGCTransition]
        private static partial int SizeOf(QCallTypeHandle handle);

        /// <summary>
        /// Get the size of an object of the given type.
        /// </summary>
        /// <param name="type">The type to get the size of.</param>
        /// <returns>The size of instances of the type.</returns>
        /// <exception cref="ArgumentException">The passed-in type is not a valid type to get the size of.</exception>
        /// <remarks>
        /// This API returns the same value as <c>sizeof(T)</c> for the type that <paramref name="type"/> represents.
        /// </remarks>
        public static int SizeOf(RuntimeTypeHandle type)
        {
            if (type.IsNullHandle())
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.type);

            int result = SizeOf(new QCallTypeHandle(ref type));

            if (result <= 0)
                throw new ArgumentException(SR.Arg_TypeNotSupported);

            return result;
        }

        [UnmanagedCallersOnly]
        internal static unsafe void CallToString(object* pObj, string* pResult, Exception* pException)
        {
            try
            {
                *pResult = pObj->ToString();
            }
            catch (Exception ex)
            {
                *pException = ex;
            }
        }

        // Dummy method providing a MethodDesc with the correct signature (IntPtr -> object)
        // for newobj allocator JIT helpers on portable entry point platforms. The interpreter
        // uses the MethodDesc to derive the call cookie; the method itself is never executed.
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object NewobjHelperDummy(IntPtr methodTable);

        [UnmanagedCallersOnly]
        internal static unsafe void CallDefaultConstructor(object* pObj, delegate*<object, void> pCtor, Exception* pException)
        {
            try
            {
                pCtor(*pObj);
            }
            catch (Exception ex)
            {
                *pException = ex;
            }
        }
    }
    // Helper class to assist with unsafe pinning of arbitrary objects.
    // It's used by VM code.
    [NonVersionable] // This only applies to field layout
    internal sealed class RawData
    {
        public byte Data;
    }

    // CLR arrays are laid out in memory as follows (multidimensional array bounds are optional):
    // [ sync block || pMethodTable || num components || MD array bounds || array data .. ]
    //                 ^               ^                 ^                  ^ returned reference
    //                 |               |                 \-- ref Unsafe.As<RawArrayData>(array).Data
    //                 \-- array       \-- ref Unsafe.As<RawData>(array).Data
    // The BaseSize of an array includes all the fields before the array data,
    // including the sync block and method table. The reference to RawData.Data
    // points at the number of components, skipping over these two pointer-sized fields.
    [NonVersionable] // This only applies to field layout
    internal sealed class RawArrayData
    {
        public uint Length; // Array._numComponents padded to IntPtr
#if TARGET_64BIT
        public uint Padding;
#endif
        public byte Data;
    }

    // Subset of src\vm\methoddesc.hpp
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MethodDesc
    {
        public ushort Flags3AndTokenRemainder;
        public byte ChunkIndex;
        public byte Flags4; // Used to hold more flags
        public ushort SlotNumber; // The slot number of this MethodDesc in the vtable array.
        public ushort Flags; // See MethodDescFlags
        public IntPtr CodeData;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerHidden]
        [DebuggerStepThrough]
        private MethodDescChunk* GetMethodDescChunk() => (MethodDescChunk*)(((byte*)Unsafe.AsPointer<MethodDesc>(ref this)) - (sizeof(MethodDescChunk) + ChunkIndex * sizeof(IntPtr)));

        public MethodTable* MethodTable => GetMethodDescChunk()->MethodTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MethodDescChunk
    {
        public MethodTable* MethodTable;
        public MethodDescChunk*  Next;
        public byte Size;        // The size of this chunk minus 1 (in multiples of MethodDesc::ALIGNMENT)
        public byte Count;       // The number of MethodDescs in this chunk minus 1
        public ushort FlagsAndTokenRange;
    }

    // Tachyon: CoreCLR's MethodTable is a native layout the BCL reads by field
    // offset. Under Tachyon a type handle is the compiler's RuntimeType metadata,
    // whose layout only the compiler knows, so nothing is read here: every member
    // is answered by a TypeHandle_Get* service the compiler provides — a function
    // in the JIT host (MethodManager.RegisterTypeInitServices) and code emitted
    // into an AOT image (ClrHost.AotTypeInit), each one field of the metadata,
    // filled when the type is finalized (RuntimeType.ComputeHandleData). The flag
    // bits are Witschi.NetClr.TS.TypeHandleFlag; change both or neither. A member
    // the runtime does not answer is an InternalCall with no implementation, so a
    // path that reaches it fails by name rather than reading nonsense (M-48 in the
    // Witchcraft managed roadmap says which those are).
    internal unsafe partial struct MethodTable
    {
        // Bits of TypeHandle_GetFlags: Witschi.NetClr.TS.TypeHandleFlag.
        private const int flag_ValueType = 0x0001;
        private const int flag_Primitive = 0x0002;             // primitives and enums, as IsPrimitive counts them
        private const int flag_TruePrimitive = 0x0004;
#pragma warning disable CA1823 // unread, kept so the bits match TypeHandleFlag
        private const int flag_Enum = 0x0008;
#pragma warning restore CA1823
        private const int flag_Interface = 0x0010;
        private const int flag_SzArray = 0x0020;
        private const int flag_MdArray = 0x0040;
        private const int flag_String = 0x0080;
        private const int flag_Nullable = 0x0100;
        private const int flag_ByRefLike = 0x0200;
        private const int flag_GenericTypeDefinition = 0x0400;
        private const int flag_HasInstantiation = 0x0800;
        private const int flag_ContainsGenericVariables = 0x1000;
        private const int flag_HasDefaultCtor = 0x2000;
        private const int flag_ContainsGCPointers = 0x4000;
        private const int flag_HasFinalizer = 0x8000;

        // The header Tachyon.Runtime.ObjectHeap keeps below every object, counted in
        // BaseSize as CoreCLR counts its own.
        private const uint ObjectHeaderSize = 8;

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetFlags")]
        private static partial int GetFlags(void* typeHnd);

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetComponentSize")]
        private static partial int GetComponentSize(void* typeHnd);

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetBaseSize")]
        private static partial uint GetBaseSize(void* typeHnd);

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetRank")]
        private static partial int GetRank(void* typeHnd);

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetParent")]
        private static partial void* GetParent(void* typeHnd);

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetElementType")]
        private static partial void* GetElementType(void* typeHnd);

        /// <summary>The metadata this method table is: its own address.</summary>
        private void* Handle => Unsafe.AsPointer(ref this);

        private int Flags => GetFlags(Handle);

        /// <summary>An array's element stride, 2 for a string, 0 for everything else.</summary>
        public ushort ComponentSize => (ushort)GetComponentSize(Handle);

        /// <summary>The object header plus the instance, so an object's data is <c>BaseSize - header - pointer</c> bytes.</summary>
        public uint BaseSize => GetBaseSize(Handle);

        public MethodTable* ParentMethodTable => (MethodTable*)GetParent(Handle);

        /// <summary>The element type of an array, pointer or byref type; null otherwise.</summary>
        public void* ElementType => GetElementType(Handle);

        // Not answered: interface maps, generic dictionaries, the auxiliary data and
        // the nullable unbox layout are CoreCLR's; nothing on a supported path reads
        // them, and a path that does fails by the extern's name.
        public ushort InterfaceCount => GetInterfaceCount();
        public MethodTableAuxiliaryData* AuxiliaryData => GetAuxiliaryData();
        public MethodTable*** PerInstInfo => GetPerInstInfo();
        public MethodTable** InterfaceMap => GetInterfaceMap();
        public uint NullableValueAddrOffset => GetNullableValueAddrOffset();
        public uint NullableValueSize => GetNullableValueSize();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort GetInterfaceCount();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern MethodTableAuxiliaryData* GetAuxiliaryData();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern MethodTable*** GetPerInstInfo();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern MethodTable** GetInterfaceMap();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern uint GetNullableValueAddrOffset();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern uint GetNullableValueSize();

        public bool HasComponentSize => (Flags & (flag_SzArray | flag_MdArray | flag_String)) != 0;

        public bool ContainsGCPointers => (Flags & flag_ContainsGCPointers) != 0;

        // Arrays and value types, as CoreCLR's category bits; no COM, no IDynamicInterfaceCastable.
        public bool NonTrivialInterfaceCast => (Flags & (flag_SzArray | flag_MdArray | flag_ValueType)) != 0;

#if FEATURE_TYPEEQUIVALENCE
        public bool HasTypeEquivalence => false;
#endif // FEATURE_TYPEEQUIVALENCE

#if FEATURE_OBJCMARSHAL
        public bool IsTrackedReferenceWithFinalizer => false;
#endif // FEATURE_OBJCMARSHAL

        public bool HasFinalizer => (Flags & flag_HasFinalizer) != 0;

#pragma warning disable CA1822 // instance member: callers use mt->IsCollectible
        public bool IsCollectible => false;
#pragma warning restore CA1822

        internal static bool AreSameType(MethodTable* mt1, MethodTable* mt2) => mt1 == mt2;

        public bool HasDefaultConstructor => (Flags & flag_HasDefaultCtor) != 0;

        public bool IsSzArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (Flags & flag_SzArray) != 0;
        }

        public bool IsMultiDimensionalArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (Flags & flag_MdArray) != 0;
        }

        // Returns rank of multi-dimensional array rank, 0 for sz arrays
        public int MultiDimensionalArrayRank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetRank(Handle);
        }

        public bool IsInterface => (Flags & flag_Interface) != 0;

        public bool IsValueType => (Flags & flag_ValueType) != 0;

        public bool IsNullable { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return (Flags & flag_Nullable) != 0; } }

        public bool IsByRefLike => (Flags & flag_ByRefLike) != 0;

        // Warning! UNLIKE the similarly named Reflection api, this method also returns "true" for Enums.
        public bool IsPrimitive => (Flags & flag_Primitive) != 0;

        public bool IsTruePrimitive => (Flags & flag_TruePrimitive) != 0;

        public bool IsArray => (Flags & (flag_SzArray | flag_MdArray)) != 0;

        public bool HasInstantiation => (Flags & flag_HasInstantiation) != 0;

        public bool IsGenericTypeDefinition => (Flags & flag_GenericTypeDefinition) != 0;

        // Every instantiation is its own type here; there is no shared canonical form.
        public bool IsConstructedGenericType => (Flags & flag_HasInstantiation) != 0;

#pragma warning disable CA1822 // instance member: callers use mt->IsSharedByGenericInstantiations
        public bool IsSharedByGenericInstantiations => false;
#pragma warning restore CA1822

        public bool ContainsGenericVariables => (Flags & flag_ContainsGenericVariables) != 0;

        /// <summary>
        /// Gets a <see cref="TypeHandle"/> for the element type of the current type.
        /// </summary>
        /// <remarks>This method should only be called when the current <see cref="MethodTable"/> instance represents an array or string type.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeHandle GetArrayElementTypeHandle()
        {
            Debug.Assert(HasComponentSize);

            return new(ElementType);
        }

        /// <summary>The instance's bytes after its type word: a value type's laid-out size, a class's fields.</summary>
        public uint GetNumInstanceFieldBytes() => BaseSize - (ObjectHeaderSize + (uint)sizeof(IntPtr));

        /// <summary>
        /// Get the <see cref="CorElementType"/> representing primitive-like type. Enums are represented by underlying type.
        /// </summary>
        /// <remarks>This method should only be called when <see cref="IsPrimitive"/> returns <see langword="true"/>.</remarks>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CorElementType GetPrimitiveCorElementType();

        /// <summary>
        /// Get the MethodTable in the type hierarchy of this MethodTable that has the same TypeDef/Module as parent.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MethodTable* GetMethodTableMatchingParentClass(MethodTable* parent);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MethodTable* InstantiationArg0();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetNullableNumInstanceFieldBytes()
        {
            Debug.Assert(IsNullable);
            return NullableValueAddrOffset + NullableValueSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetNumInstanceFieldBytesIfContainsGCPointers()
        {
            Debug.Assert(ContainsGCPointers);
            return GetNumInstanceFieldBytes();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IntPtr GetLoaderAllocatorHandle();
    }

    // Subset of src\vm\typedesc.h
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct TypeDesc
    {
        private uint _typeAndFlags;
        private nint _exposedClassObject;

        private const uint enum_flag_IsCollectible = 0x00000100;

        public RuntimeType? ExposedClassObject
        {
            get
            {
                return *(RuntimeType*)Unsafe.AsPointer(ref _exposedClassObject);
            }
        }

        public bool IsCollectible
        {
            get
            {
                return (_typeAndFlags & enum_flag_IsCollectible) != 0;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe ref struct DynamicStaticsInfo
    {
        internal const int ISCLASSNOTINITED = 1;
        internal IntPtr _pGCStatics; // The ISCLASSNOTINITED bit is set when the class is NOT initialized
        internal IntPtr _pNonGCStatics; // The ISCLASSNOTINITED bit is set when the class is NOT initialized

        /// <summary>
        /// Given a statics pointer in the DynamicStaticsInfo, get the actual statics pointer.
        /// If the class it initialized, this mask is not necessary
        /// </summary>
        [DebuggerHidden]
        [DebuggerStepThrough]
        internal static ref byte MaskStaticsPointer(ref byte staticsPtr)
        {
            fixed (byte* p = &staticsPtr)
            {
                return ref Unsafe.AsRef<byte>((byte*)((nuint)p & ~(nuint)DynamicStaticsInfo.ISCLASSNOTINITED));
            }
        }

        internal MethodTable* _methodTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal ref struct GenericsStaticsInfo
    {
        // Pointer to field descs for statics
        internal IntPtr _pFieldDescs;
        internal DynamicStaticsInfo _dynamicStatics;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal ref struct ThreadStaticsInfo
    {
        internal int _nonGCTlsIndex;
        internal int _gcTlsIndex;
        internal GenericsStaticsInfo _genericStatics;
    }


    // Subset of src\vm\methodtable.h
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MethodTableAuxiliaryData
    {
        private uint Flags;
        private int CachedVersionResilientHashCode;
        private void* LoaderModule;
        private nint ExposedClassObjectRaw;

        private const uint enum_flag_HasCheckedCanCompareBitsOrUseFastGetHashCode = 0x0002;  // Whether we have checked the overridden Equals or GetHashCode
        private const uint enum_flag_CanCompareBitsOrUseFastGetHashCode = 0x0004;     // Is any field type or sub field type overridden Equals or GetHashCode

        private const uint enum_flag_Initialized                = 0x0001;
        private const uint enum_flag_HasCheckedStreamOverride   = 0x0400;
        private const uint enum_flag_StreamOverriddenRead       = 0x0800;
        private const uint enum_flag_StreamOverriddenWrite      = 0x1000;
        private const uint enum_flag_EnsuredInstanceActive      = 0x2000;


        public bool HasCheckedCanCompareBitsOrUseFastGetHashCode => (Flags & enum_flag_HasCheckedCanCompareBitsOrUseFastGetHashCode) != 0;

        public bool CanCompareBitsOrUseFastGetHashCode
        {
            get
            {
                Debug.Assert(HasCheckedCanCompareBitsOrUseFastGetHashCode);
                return (Flags & enum_flag_CanCompareBitsOrUseFastGetHashCode) != 0;
            }
        }

        public bool HasCheckedStreamOverride => (Flags & enum_flag_HasCheckedStreamOverride) != 0;

        public bool IsStreamOverriddenRead
        {
            get
            {
                Debug.Assert(HasCheckedStreamOverride);
                return (Flags & enum_flag_StreamOverriddenRead) != 0;
            }
        }

        public bool IsStreamOverriddenWrite
        {
            get
            {
                Debug.Assert(HasCheckedStreamOverride);
                return (Flags & enum_flag_StreamOverriddenWrite) != 0;
            }
        }

        public RuntimeType? ExposedClassObject
        {
            get
            {
                return *(RuntimeType*)Unsafe.AsPointer(ref ExposedClassObjectRaw);
            }
        }

        public bool IsClassInited => (Volatile.Read(ref Flags) & enum_flag_Initialized) != 0;

        public bool IsClassInitedAndActive => (Volatile.Read(ref Flags) & (enum_flag_Initialized | enum_flag_EnsuredInstanceActive)) == (enum_flag_Initialized | enum_flag_EnsuredInstanceActive);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public ref DynamicStaticsInfo GetDynamicStaticsInfo()
        {
            return ref Unsafe.Subtract(ref Unsafe.As<MethodTableAuxiliaryData, DynamicStaticsInfo>(ref this), 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public ref ThreadStaticsInfo GetThreadStaticsInfo()
        {
            return ref Unsafe.Subtract(ref Unsafe.As<MethodTableAuxiliaryData, ThreadStaticsInfo>(ref this), 1);
        }
    }

    /// <summary>
    /// A type handle, which can wrap either a pointer to a <c>TypeDesc</c> or to a <see cref="MethodTable"/>.
    /// </summary>
    internal readonly unsafe partial struct TypeHandle
    {
        // Subset of src\vm\typehandle.h

        /// <summary>
        /// The address of the current type handle object.
        /// </summary>
        private readonly void* m_asTAddr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeHandle(void* tAddr)
        {
            m_asTAddr = tAddr;
        }

        /// <summary>
        /// Gets whether the current instance wraps a <see langword="null"/> pointer.
        /// </summary>
        public bool IsNull => m_asTAddr is null;

        /// <summary>
        /// Gets whether or not this <see cref="TypeHandle"/> wraps a <c>TypeDesc</c> pointer.
        /// Only if this returns <see langword="false"/> it is safe to call <see cref="AsMethodTable"/>.
        /// </summary>
        public bool IsTypeDesc
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((nint)m_asTAddr & 2) != 0;
        }

        /// <summary>
        /// Gets the <see cref="MethodTable"/> pointer wrapped by the current instance.
        /// </summary>
        /// <remarks>This is only safe to call if <see cref="IsTypeDesc"/> returned <see langword="false"/>.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MethodTable* AsMethodTable()
        {
            Debug.Assert(!IsTypeDesc);

            return (MethodTable*)m_asTAddr;
        }

        /// <summary>
        /// Gets the <see cref="TypeDesc"/> pointer wrapped by the current instance.
        /// </summary>
        /// <remarks>This is only safe to call if <see cref="IsTypeDesc"/> returned <see langword="true"/>.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeDesc* AsTypeDesc()
        {
            Debug.Assert(IsTypeDesc);

            return (TypeDesc*)((nint)m_asTAddr & ~2); // Drop the second lowest bit.
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeHandle TypeHandleOf<T>()
        {
            return new TypeHandle((void*)RuntimeTypeHandle.ToIntPtr(typeof(T).TypeHandle));
        }

        public static bool AreSameType(TypeHandle left, TypeHandle right) => left.m_asTAddr == right.m_asTAddr;

        public int GetCorElementType() => GetCorElementType(m_asTAddr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanCastTo(TypeHandle destTH)
        {
            return TryCanCastTo(this, destTH) switch
            {
                CastResult.CanCast => true,
                CastResult.CannotCast => false,

                // Regular casting does not allow T to be cast to Nullable<T>.
                // See TypeHandle::CanCastTo()
                _ => CanCastToWorker(this, destTH, nullableCast: false)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanCastToForReflection(TypeHandle srcTH, TypeHandle destTH)
        {
            return TryCanCastTo(srcTH, destTH) switch
            {
                CastResult.CanCast => true,
                CastResult.CannotCast => false,

                // Reflection allows T to be cast to Nullable<T>.
                // See ObjIsInstanceOfCore()
                _ => CanCastToWorker(srcTH, destTH, nullableCast: true)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CastResult TryCanCastTo(TypeHandle srcTH, TypeHandle destTH)
        {
            // See TypeHandle::CanCastToCached() for duplicate quick checks.
            if (srcTH.m_asTAddr == destTH.m_asTAddr)
                return CastResult.CanCast;

            if (!srcTH.IsTypeDesc && destTH.IsTypeDesc)
                return CastResult.CannotCast;

            return CastCache.TryGet(CastHelpers.s_table!, (nuint)srcTH.m_asTAddr, (nuint)destTH.m_asTAddr);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool CanCastToWorker(TypeHandle srcTH, TypeHandle destTH, bool nullableCast)
        {
            if (!srcTH.IsTypeDesc
                && !destTH.IsTypeDesc
                && CastHelpers.IsNullableForType(destTH.AsMethodTable(), srcTH.AsMethodTable()))
            {
                return nullableCast;
            }

            return CanCastTo_NoCacheLookup(srcTH.m_asTAddr, destTH.m_asTAddr) != Interop.BOOL.FALSE;
        }

        [ErrorHandler(typeof(QCallExceptionStatusMarshaller), ErrorLocation.HiddenLastParameter)]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_CanCastTo_NoCacheLookup")]
        private static partial Interop.BOOL CanCastTo_NoCacheLookup(void* fromTypeHnd, void* toTypeHnd);

        [SuppressGCTransition]
        [LibraryImport(RuntimeHelpers.QCall, EntryPoint = "TypeHandle_GetCorElementType")]
        private static partial int GetCorElementType(void* typeHnd);
    }

    // Helper structs used for tail calls via helper.
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct PortableTailCallFrame
    {
        public IntPtr TailCallAwareReturnAddress;
        public delegate*<TailCallArgBuffer*, ref byte, PortableTailCallFrame*, void> NextCall;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TailCallArgBuffer
    {
        public int State;
        public int Size;
        public IntPtr GCDesc;
        // Args
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct TailCallTls
    {
        public PortableTailCallFrame* Frame;
        public TailCallArgBuffer* ArgBuffer;
    }
}
