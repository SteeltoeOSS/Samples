#!/bin/bash
function printUsage()
{
echo "USAGE:" 
echo "pushMusicStoreUI [runtime] [framework]"
echo "runtime - target runtime to publish (e.g. win10-x64, ubuntu.16.04-x64)"
echo "framework - target framework to publish (e.g. net461, netcoreapp2.1)"
exit
}
#
if  [ "$1" == "" ]; then 
	printUsage 
fi
if [ "$2" == "" ]; then
	printUsage 
fi
r=$1
cd src/MusicStoreUI
nixmanifest=manifest.yml
winmanifest=manifest-windows.yml
if [ "$USE_REDIS_CACHE" == "true" ]; then
	nixmanifest=manifest-redis.yml
	winmanifest=manifest-windows-redis.yml
fi

dotnet restore --configfile nuget.config
dotnet publish --configuration Release --runtime "$1"  --framework "$2"
if [ "${r:0:3}" == "win" ]; then 
	cf push -f "$winmanifest" -p "bin/Release/$2/$1/publish" 
fi
if [ "${r:0:6}" == "ubuntu" ]; then
	cf push -f "$nixmanifest" -p "bin/Release/$2/$1/publish" 
fi
cd ../..
