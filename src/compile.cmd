@echo off
setlocal enabledelayedexpansion
echo ================================
echo   Publishing Avalonia App
echo   .NET 10 Optimized Build
echo ================================
echo.

set OUTDIR=publish
set ERROR_FLAG=0

if exist %OUTDIR% (
    echo Cleaning previous publish output...
    rmdir /s /q %OUTDIR%
)
echo.

echo === Windows x86 (self-contained, trimmed) ===
dotnet publish -c Release -r win-x86 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:PublishTrimmed=true ^
  -p:TrimMode=link ^
  -p:EnableCompressionInSingleFile=true ^
  -p:DebugType=none ^
  -p:DebugSymbols=false ^
  -p:StripSymbols=true ^
  -o %OUTDIR%\win-x86

if errorlevel 1 (
    echo Windows x86 publish failed!
    set ERROR_FLAG=1
)
echo.

echo === Linux x64 (self-contained, trimmed) ===
dotnet publish -c Release -r linux-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:PublishTrimmed=true ^
  -p:TrimMode=link ^
  -p:EnableCompressionInSingleFile=true ^
  -p:DebugType=none ^
  -p:DebugSymbols=false ^
  -p:StripSymbols=true ^
  -o %OUTDIR%\linux-x64

if errorlevel 1 (
    echo Linux x64 publish failed!
    set ERROR_FLAG=1
)
echo.

echo === Linux ARM64 (self-contained, trimmed) ===
dotnet publish -c Release -r linux-arm64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:PublishTrimmed=true ^
  -p:TrimMode=link ^
  -p:EnableCompressionInSingleFile=true ^
  -p:DebugType=none ^
  -p:DebugSymbols=false ^
  -p:StripSymbols=true ^
  -o %OUTDIR%\linux-arm64

if errorlevel 1 (
    echo Linux ARM64 publish failed!
    set ERROR_FLAG=1
)
echo.

if %ERROR_FLAG%==0 (
    echo ================================
    echo   Publish completed successfully
    echo   Output folder: %OUTDIR%
    echo ================================
    echo.
    echo File sizes:
    for %%F in (%OUTDIR%\win-x64\*.exe) do echo   win-x64:      %%~zF bytes (%%~nxF)
    for %%F in (%OUTDIR%\win-x86\*.exe) do echo   win-x86:      %%~zF bytes (%%~nxF)
    for %%F in (%OUTDIR%\linux-x64\ARM9Editor) do echo   linux-x64:    %%~zF bytes
    for %%F in (%OUTDIR%\linux-arm64\ARM9Editor) do echo   linux-arm64:  %%~zF bytes
) else (
    echo ================================
    echo   Publish completed with errors
    echo   Some targets failed
    echo ================================
)
echo.
pause