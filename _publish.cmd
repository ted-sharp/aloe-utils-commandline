@echo off
setlocal enabledelayedexpansion

cd /d %~dp0

echo Publishing Aloe.Utils.ArgsHelper...

rem Clean previous publish directory
if exist "publish" (
    echo Removing previous publish directory...
    rmdir /s /q publish
)

rem Publish the application
echo Building and publishing...
dotnet publish .\Aloe.Utils.ArgsHelper\Aloe.Utils.ArgsHelper.csproj -c Release -r win-x64 -o .\publish\AloeUtilsArgsHelper

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Publish completed successfully.
) else (
    echo.
    echo Publish failed with error code %ERRORLEVEL%
)

pause
