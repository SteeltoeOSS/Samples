#!/bin/bash
function printUsage()
{
echo "USAGE:" 
echo "pushMusicStoreUI [runtime] [framework]"
echo "runtime - target runtime to publish (e.g. win7-x64, ubuntu.14.04-x64)"
echo "framework - target framework to publish (e.g. net451, netcoreapp1.0)"
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
if [ -d "$PWD/publish" ]; then
	rm -rf "$PWD/publish" 
fi
dotnet restore --configfile nuget.config
dotnet publish --output $PWD/publish --configuration Release --runtime "$1"  --framework "$2"
if [ "${r:0:3}" == "win" ]; then 
	cf push -f manifest-windows.yml -p "$PWD/publish" 
fi
if [ "${r:0:6}" == "ubuntu" ]; then
	cf push -f manifest.yml -p "$PWD/publish" 
fi
cd ../..