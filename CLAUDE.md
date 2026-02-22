# Tachyon .NET Runtime

## What This Is
A fork of dotnet/runtime that adds the "Tachyon" platform target for embedded environments. Only managed code is built (CoreLib + framework libraries) — no native runtime components.

## Branch
- `TachyonSupport` — all Tachyon work lives here, rebased onto dotnet/main (net11.0)

## Build
```cmd
# Single architecture
build.cmd -subset clr.corelib -os tachyon -arch x86 -c Debug
build.cmd -subset libs.sfx -os tachyon -arch x86 -c Debug

# All architectures + NuGet package
src\tachyon\build-and-pack.cmd [Config] [Version]
```

Supported architectures: x86, x64, arm, arm64 (all cross-compile on Windows x64).

## Key Conventions
- Platform shims go in `*.Tachyon.cs` partial class files (same pattern as WASI/Browser/Haiku)
- `TargetOS` is lowercase `tachyon` everywhere
- Use `PlatformNotSupportedException` for fundamentally unsupported features
- Use `NotImplementedException` for features that could be implemented later
- Public API stubs must include ALL platform attributes matching Unix/Windows implementations (for APICompat)

## Interop / Runtime Calls Pattern
Tachyon does NOT use the standard `Interop.Sys.*` / `Interop.Ucrtbase.*` DllImport pattern. Instead:

- **Interop declarations** go in `src/libraries/Common/src/Interop/Tachyon/Tachyon.<Feature>.cs`
  - Namespace: `Tachyon` (not `Interop`)
  - Class: `internal static unsafe partial class <Feature>`
  - Methods: `[LibraryImport("*")]` with `internal static partial` signatures
  - `[DllImport]` is NOT allowed — CoreLib enforces `LibraryImportAttribute` (SYSLIB1054)
  - Classes must be `internal` (not `public`) to avoid CLS compliance errors
- **Managed implementations** go in `*.Tachyon.cs` partial class files (existing pattern)
  - Put all validation, edge-case handling, and error checking in managed code
  - Keep native calls as thin primitives — the managed side does the heavy lifting
  - Call pattern: `Tachyon.<ClassName>.<Method>()` (e.g., `Tachyon.NativeMemory.Alloc()`)
- **Build registration**: New interop files must be added to `System.Private.CoreLib.Shared.projitems` in the `'$(TargetsTachyon)' == 'true'` ItemGroup with a `<Link>` element
- Reference Unix/Windows implementations for the validation logic, then simplify where Tachyon's runtime handles edge cases natively

## Project Structure
- `src/tachyon/` — build scripts and NuGet packaging
- `src/libraries/Common/src/Interop/Tachyon/` — runtime interop declarations (`Tachyon.<Feature>.cs`)
- `eng/RuntimeIdentifier.props` — defines `TargetsTachyon` property
- `.github/workflows/tachyon-build.yml` — CI: builds all arches, publishes NuGet to GitHub Packages
- Shim files scattered across `src/coreclr/` and `src/libraries/` as `*.Tachyon.cs`

## CI
GitHub Actions workflow triggers on push to `TachyonSupport` or manual dispatch. Builds all 4 architectures, packs NuGet, publishes to GitHub Packages (`https://nuget.pkg.github.com/Rew/index.json`).
