#!/bin/bash
function printUsage()
{
echo "USAGE:" 
echo "pushOrderService [runtime] [framework]"
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
cd src/OrderService

dotnet restore --configfile nuget.config
dotnet publish --configuration Release --runtime "$1"  --framework "$2"
if [ "${r:0:3}" == "win" ]; then 
	cf push -f manifest-windows.yml -p "bin/Release/$2/$1/publish" 
fi
if [ "${r:0:6}" == "ubuntu" ]; then
	cf push -f manifest.yml -p "bin/Release/$2/$1/publish"  
fi
cd ../..