@echo off
setlocal enabledelayedexpansion

set REPO_ROOT=%~dp0..\..
set CONFIG=%1
if "%CONFIG%"=="" set CONFIG=Debug
set PKG_VERSION=%2
if "%PKG_VERSION%"=="" (
    for /f %%h in ('git -C "%REPO_ROOT%" rev-parse --short HEAD') do set GIT_SHORT_SHA=%%h
    set PKG_VERSION=11.0.0-dev.!GIT_SHORT_SHA!
)

set ARCHITECTURES=x86 x64 arm arm64

echo ============================================================
echo  Tachyon .NET Framework Build
echo  Configuration: %CONFIG%
echo  Architectures: %ARCHITECTURES%
echo ============================================================

set STEP=0
set TOTAL=0
for %%A in (%ARCHITECTURES%) do set /a TOTAL+=2
set /a TOTAL+=1

for %%A in (%ARCHITECTURES%) do (
    set /a STEP+=1
    echo.
    echo [!STEP!/%TOTAL%] Building CoreLib [%%A]...
    call %REPO_ROOT%\build.cmd -subset clr.corelib -os tachyon -arch %%A -c %CONFIG%
    if errorlevel 1 (
        echo ERROR: CoreLib build failed for %%A.
        exit /b 1
    )

    set /a STEP+=1
    echo.
    echo [!STEP!/%TOTAL%] Building framework libraries [%%A]...
    call %REPO_ROOT%\build.cmd -subset libs.sfx -os tachyon -arch %%A -c %CONFIG%
    if errorlevel 1 (
        echo ERROR: Framework library build failed for %%A.
        exit /b 1
    )
)

set /a STEP+=1
echo.
echo [%STEP%/%TOTAL%] Packaging...

set PKG_OUT=%REPO_ROOT%\artifacts\packages\%CONFIG%\tachyon

%REPO_ROOT%\.dotnet\dotnet.exe pack "%~dp0pkg\Tachyon.BCL.Artifacts.csproj" ^
    -o "%PKG_OUT%" ^
    /p:PackageVersion=%PKG_VERSION% ^
    /p:TachyonConfig=%CONFIG% ^
    /p:TachyonArchitectures="x86;x64;arm;arm64"

if errorlevel 1 (
    echo ERROR: Packaging failed.
    exit /b 1
)

echo.
echo ============================================================
echo  Build complete!
echo  Package: %PKG_OUT%\Tachyon.BCL.Artifacts.%PKG_VERSION%.nupkg
echo ============================================================
