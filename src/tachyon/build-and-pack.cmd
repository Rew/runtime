@echo off
setlocal

set REPO_ROOT=%~dp0..\..
set CONFIG=%1
if "%CONFIG%"=="" set CONFIG=Debug
set ARCH=%2
if "%ARCH%"=="" set ARCH=x86
set PKG_VERSION=%3
if "%PKG_VERSION%"=="" set PKG_VERSION=11.0.0-dev

echo ============================================================
echo  Tachyon .NET Framework Build
echo  Configuration: %CONFIG%  Architecture: %ARCH%
echo ============================================================

echo.
echo [1/3] Building CoreLib...
call %REPO_ROOT%\build.cmd -subset clr.corelib -os tachyon -arch %ARCH% -c %CONFIG%
if errorlevel 1 (
    echo ERROR: CoreLib build failed.
    exit /b 1
)

echo.
echo [2/3] Building framework libraries...
call %REPO_ROOT%\build.cmd -subset libs.sfx -os tachyon -arch %ARCH% -c %CONFIG%
if errorlevel 1 (
    echo ERROR: Framework library build failed.
    exit /b 1
)

echo.
echo [3/3] Packaging...

set PKG_OUT=%REPO_ROOT%\artifacts\packages\%CONFIG%\tachyon

%REPO_ROOT%\.dotnet\dotnet.exe pack "%~dp0pkg\Tachyon.NET.Runtime.csproj" ^
    -o "%PKG_OUT%" ^
    /p:PackageVersion=%PKG_VERSION% ^
    /p:TachyonConfig=%CONFIG% ^
    /p:TachyonArch=%ARCH%

if errorlevel 1 (
    echo ERROR: Packaging failed.
    exit /b 1
)

echo.
echo ============================================================
echo  Build complete!
echo  Package: %PKG_OUT%\Tachyon.NET.Runtime.%PKG_VERSION%.nupkg
echo ============================================================
