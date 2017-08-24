@ECHO OFF
IF "%1"=="" GOTO :usage
IF "%2"=="" GOTO :usage
SET r=%1
cd src\MusicStoreUI
IF EXIST %TEMP%\publish (CMD /C "RMDIR /q /s %TEMP%\publish")
set winmanifest=manifest-windows.yml
IF NOT "%USE_REDIS_CACHE%"=="" (set winmanifest=manifest-windows-redis.yml)
set nixmanifest=manifest.yml
IF NOT "%USE_REDIS_CACHE%"=="" (set nixmanifest=manifest-redis.yml)
dotnet restore
dotnet publish --output %TEMP%\publish --configuration Release --runtime %1 --framework %2
IF "%r:~0,3%"=="win" (CMD /c "cf push -f %winmanifest% -p %TEMP%\publish")
IF "%r:~0,6%"=="ubuntu" (CMD /c "cf push -f %nixmanifest% -p %TEMP%\publish")
cd ..\..
exit /b
:usage
echo USAGE: 
echo pushMusicStoreUI [runtime] [framework]
echo runtime - target runtime to publish (e.g. win10-x64, ubuntu.14.04-x64)
echo framework - target framework to publish (e.g. net461, netcoreapp1.1)
exit /b