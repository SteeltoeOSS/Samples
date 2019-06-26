@ECHO OFF
IF "%1"=="" GOTO :usage
IF "%2"=="" GOTO :usage
SET r=%1
cd src\MusicStoreUI
IF NOT "%USE_REDIS_CACHE%"=="" (set DefineConstants=USE_REDIS_CACHE)
set winmanifest=manifest-windows.yml
IF NOT "%USE_REDIS_CACHE%"=="" (set winmanifest=manifest-windows-redis.yml)
set nixmanifest=manifest.yml
IF NOT "%USE_REDIS_CACHE%"=="" (set nixmanifest=manifest-redis.yml)
dotnet restore
dotnet publish --configuration Release --runtime %1 --framework %2
IF "%r:~0,3%"=="win" (CMD /c "cf push -f %winmanifest% -p bin\Release\%2\%1\publish")
IF "%r:~0,6%"=="ubuntu" (CMD /c "cf push -f %nixmanifest% -p bin\Release\%2\%1\publish")
cd ..\..
exit /b
:usage
echo USAGE: 
echo pushMusicStoreUI [runtime] [framework]
echo runtime - target runtime to publish (e.g. win10-x64, ubuntu.16.04-x64)
echo framework - target framework to publish (e.g. net461, netcoreapp2.1)
exit /b