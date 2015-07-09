@echo Off
SET config=%1
if "%config%" == "" (
   SET config=Release
)

CALL "%~dp0setmsbuild.cmd"

call "%~dp0RestorePackages.cmd"
echo %msbuild% "%~dp0Nz.Co.Dunkley.Cornerman.sln" /nologo /verbosity:m /t:Rebuild /p:MvcBuildViews=True;Configuration="%config%"
%msbuild% "%~dp0Nz.Co.Dunkley.Cornerman.sln" /nologo /verbosity:m /t:Rebuild /p:MvcBuildViews=True;Configuration="%config%"
