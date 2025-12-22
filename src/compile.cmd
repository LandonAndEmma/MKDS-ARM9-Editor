@echo off
setlocal enabledelayedexpansion

:: ============================================
:: Avalonia Application Publisher
:: .NET 10 Optimized Build Script
:: ============================================

echo.
echo ============================================
echo     Avalonia Application Publisher
echo     .NET 10 Optimized Build
echo ============================================
echo.

:: Configuration
set "PUBLISH_DIR=publish"
set "PROJECT_NAME=ARM9Editor"
set "ERROR_COUNT=0"
set "START_TIME=%TIME%"

:: Clean previous publish output
if exist "%PUBLISH_DIR%" (
    echo [INFO] Cleaning previous publish directory...
    rmdir /s /q "%PUBLISH_DIR%"
    if errorlevel 1 (
        echo [ERROR] Failed to clean publish directory
        set /a ERROR_COUNT+=1
    )
)

:: Function to publish a target
call :PublishTarget "win-x86" "false" "%PROJECT_NAME%.exe"
call :PublishTarget "linux-x64" "true" "%PROJECT_NAME%"
call :PublishTarget "linux-arm64" "true" "%PROJECT_NAME%"

:: Display results
echo.
echo ============================================
if %ERROR_COUNT% EQU 0 (
    echo   PUBLISH COMPLETED SUCCESSFULLY
    echo   Output Directory: %PUBLISH_DIR%
    echo ============================================
    echo.
    echo [INFO] Generated artifacts:
    
    :: Display file sizes
    for /f "tokens=*" %%F in ('dir /b /s "%PUBLISH_DIR%\*" ^| findstr /v "\\obj$ \\bin$"') do (
        if exist "%%F" (
            for %%I in ("%%F") do (
                set "filepath=%%~I"
                set "relpath=!filepath:%PUBLISH_DIR%\=!"
                echo   !relpath! - %%~zI bytes
            )
        )
    )
    
    :: Show build time
    call :GetTimeDiff "%START_TIME%" "%TIME%" duration
    echo.
    echo [INFO] Build time: !duration!
) else (
    echo   PUBLISH COMPLETED WITH ERRORS
    echo   Failed targets: %ERROR_COUNT%
    echo ============================================
    echo.
    echo [ERROR] Check the errors above and try again
)
echo.
pause
exit /b %ERROR_COUNT%

:: ============================================
:: Publish Target Function
:: Parameters: <runtime> <self-contained> <output-name>
:: ============================================
:PublishTarget
set "RUNTIME=%~1"
set "SELF_CONTAINED=%~2"
set "OUTPUT_NAME=%~3"
set "OUTPUT_PATH=%PUBLISH_DIR%\%RUNTIME%"

echo.
echo ============================================
echo   Publishing: %RUNTIME%
echo   Self-contained: %SELF_CONTAINED%
echo ============================================

set "SELF_CONTAINED_FLAG="
if /i "%SELF_CONTAINED%"=="true" (
    set "SELF_CONTAINED_FLAG=--self-contained true"
)

echo [INFO] Building %RUNTIME% target...
echo.

dotnet publish -c Release -r %RUNTIME% ^
  %SELF_CONTAINED_FLAG% ^
  -p:PublishSingleFile=true ^
  -p:PublishTrimmed=true ^
  -p:TrimMode=link ^
  -p:EnableCompressionInSingleFile=true ^
  -p:DebugType=none ^
  -p:DebugSymbols=false ^
  -p:StripSymbols=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:EnableCompressionInSingleFile=true ^
  -o "%OUTPUT_PATH%" ^
  --nologo ^
  --verbosity minimal

if errorlevel 1 (
    echo [ERROR] Failed to publish %RUNTIME% target!
    set /a ERROR_COUNT+=1
    goto :eof
)

:: Rename output file if needed
if exist "%OUTPUT_PATH%\%PROJECT_NAME%.exe" if not "%OUTPUT_NAME%"=="%PROJECT_NAME%.exe" (
    move "%OUTPUT_PATH%\%PROJECT_NAME%.exe" "%OUTPUT_PATH%\%OUTPUT_NAME%" >nul 2>&1
)
if exist "%OUTPUT_PATH%\%PROJECT_NAME%" if not "%OUTPUT_NAME%"=="%PROJECT_NAME%" (
    move "%OUTPUT_PATH%\%PROJECT_NAME%" "%OUTPUT_PATH%\%OUTPUT_NAME%" >nul 2>&1
)

echo [SUCCESS] %RUNTIME% published to: %OUTPUT_PATH%
goto :eof

:: ============================================
:: Calculate time difference function
:: ============================================
:GetTimeDiff
set "start=%~1"
set "end=%~2"
set "result=%~3"

set /a "start_h=1%start:~0,2%%%100, start_m=1%start:~3,2%%%100, start_s=1%start:~6,2%%%100"
set /a "end_h=1%end:~0,2%%%100, end_m=1%end:~3,2%%%100, end_s=1%end:~6,2%%%100"

set /a "diff_h=end_h-start_h, diff_m=end_m-start_m, diff_s=end_s-start_s"

if %diff_s% lss 0 set /a "diff_s+=60, diff_m-=1"
if %diff_m% lss 0 set /a "diff_m+=60, diff_h-=1"
if %diff_h% lss 0 set /a "diff_h+=24"

set "!result!=%diff_h%h %diff_m%m %diff_s%s"
goto :eof