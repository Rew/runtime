// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System.Threading
{
    public static partial class ThreadPool
    {
#if NATIVEAOT
        private const bool IsWorkerTrackingEnabledInConfig = false;
#else
        private static readonly bool IsWorkerTrackingEnabledInConfig =
            AppContextConfigHelper.GetBooleanConfig("System.Threading.ThreadPool.EnableWorkerTracking", "DOTNET_ThreadPool_EnableWorkerTracking");
#endif

        // Indicates whether the thread pool should yield the thread from the dispatch loop to the runtime periodically so that
        // the runtime may use the thread for processing other work.
        internal static bool YieldFromDispatchLoop(int currentTickCount)
        {
            _ = currentTickCount;
            return false;
        }

        internal static ThreadInt64PersistentCounter.ThreadLocalNode? GetOrCreateThreadLocalCompletionCountNode() =>
            PortableThreadPool.ThreadPoolInstance.GetOrCreateThreadLocalCompletionCountNode();

        internal static unsafe void EnsureWorkerRequested()
        {
            PortableThreadPool.ThreadPoolInstance.EnsureWorkerRequested();
        }

        public static bool SetMaxThreads(int workerThreads, int completionPortThreads) =>
            PortableThreadPool.ThreadPoolInstance.SetMaxThreads(workerThreads, completionPortThreads);

        public static void GetMaxThreads(out int workerThreads, out int completionPortThreads)
        {
            PortableThreadPool.ThreadPoolInstance.GetMaxThreads(out workerThreads, out completionPortThreads);
        }

        public static bool SetMinThreads(int workerThreads, int completionPortThreads) =>
            PortableThreadPool.ThreadPoolInstance.SetMinThreads(workerThreads, completionPortThreads);

        public static void GetMinThreads(out int workerThreads, out int completionPortThreads)
        {
            PortableThreadPool.ThreadPoolInstance.GetMinThreads(out workerThreads, out completionPortThreads);
        }

        public static void GetAvailableThreads(out int workerThreads, out int completionPortThreads)
        {
            PortableThreadPool.ThreadPoolInstance.GetAvailableThreads(out workerThreads, out completionPortThreads);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void NotifyWorkItemProgress()
        {
            PortableThreadPool.ThreadPoolInstance.NotifyWorkItemProgress();
        }

        internal static bool NotifyThreadBlocked() =>
            PortableThreadPool.ThreadPoolInstance.NotifyThreadBlocked();

        internal static void NotifyThreadUnblocked()
        {
            PortableThreadPool.ThreadPoolInstance.NotifyThreadUnblocked();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool NotifyWorkItemComplete(ThreadInt64PersistentCounter.ThreadLocalNode threadLocalCompletionCountNode, int currentTimeMs) =>
            PortableThreadPool.ThreadPoolInstance.NotifyWorkItemComplete(threadLocalCompletionCountNode, currentTimeMs);

        internal static void ReportThreadStatus(bool isWorking)
        {
            PortableThreadPool.ThreadPoolInstance.ReportThreadStatus(isWorking);
        }

        internal static RegisteredWaitHandle RegisterWaitForSingleObject(
             WaitHandle waitObject,
             WaitOrTimerCallback callBack,
             object? state,
             uint millisecondsTimeOutInterval,
             bool executeOnlyOnce,
             bool flowExecutionContext)
        {
            ArgumentNullException.ThrowIfNull(callBack);
            return PortableThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, millisecondsTimeOutInterval, executeOnlyOnce, flowExecutionContext);
        }

        /// <summary>
        /// Gets the number of thread pool threads that currently exist.
        /// </summary>
        public static int ThreadCount
        {
            get
            {
                return PortableThreadPool.ThreadPoolInstance.ThreadCount;
            }
        }

        /// <summary>
        /// Gets the number of work items that have been processed so far.
        /// </summary>
        public static long CompletedWorkItemCount
        {
            get
            {
                return PortableThreadPool.ThreadPoolInstance.CompletedWorkItemCount;
            }
        }
    }
}
