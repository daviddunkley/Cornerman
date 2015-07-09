@SETLOCAL
@SET CACHED_NUGET="%LocalAppData%\NuGet\NuGet.exe"
@SET LOCAL_NUGET="%~dp0.nuget\NuGet.exe"

@IF EXIST %CACHED_NUGET% goto copynuget
@echo Downloading latest version of NuGet.exe...
@IF NOT EXIST "%LocalAppData%\NuGet" md "%LocalAppData%\NuGet"
powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile %CACHED_NUGET:"='%"

:copynuget
@IF EXIST %LOCAL_NUGET% goto restore
@IF NOT EXIST "%~dp0.nuget" md "%~dp0.nuget"
copy %CACHED_NUGET% %LOCAL_NUGET% > nul

:restore
%LOCAL_NUGET% restore "%~dp0Nz.Co.Dunkley.Cornerman.sln"

cd /D "%~dp0Nz.Co.Dunkley.Cornerman.Api"
npm install
