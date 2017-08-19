#!/bin/bash
function printUsage()
{
echo "USAGE:" 
echo "pushMusicStoreService [runtime] [framework]"
echo "runtime - target runtime to publish (e.g. win10-x64, ubuntu.14.04-x64)"
echo "framework - target framework to publish (e.g. net461, netcoreapp2.0)"
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
cd src/MusicStoreService
if [ -d "$TMPDIR/publish" ]; then
	rm -rf "$TMPDIR/publish" 
fi
dotnet restore --configfile nuget.config
dotnet publish --output $TMPDIR/publish --configuration Release --runtime "$1"  --framework "$2"
if [ "${r:0:3}" == "win" ]; then 
	cf push -f manifest-windows.yml -p "$TMPDIR/publish" 
fi
if [ "${r:0:6}" == "ubuntu" ]; then
	cf push -f manifest.yml -p "$TMPDIR/publish" 
fi
cd ../..