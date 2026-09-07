// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata
{
    /// <summary>
    /// Tachyon: hot reload is not supported and there is no runtime to ask. The
    /// CoreCLR file answers IsSupported with the AssemblyNative_IsApplyUpdateSupported
    /// QCall from its static initializer, which every startup would reach and fail on.
    /// </summary>
    public static partial class MetadataUpdater
    {
        public static void ApplyUpdate(Assembly assembly, ReadOnlySpan<byte> metadataDelta, ReadOnlySpan<byte> ilDelta, ReadOnlySpan<byte> pdbDelta)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            throw new PlatformNotSupportedException();
        }

        internal static string GetCapabilities() => "Baseline";

        [FeatureSwitchDefinition("System.Reflection.Metadata.MetadataUpdater.IsSupported")]
        public static bool IsSupported => false;
    }
}
