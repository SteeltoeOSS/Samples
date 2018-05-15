#!/bin/bash
function printUsage()
{
    echo "USAGE:" 
    echo "RunFortuneTeller [framework]"
    echo "framework - target framework to publish (e.g. netcoreapp2.0)"
    exit
}
#
if  [ "$1" == "" ]; then 
	printUsage 
fi
cd Fortune-Teller-Service
dotnet run -f $1 --force &
cd ../Fortune-Teller-UI
dotnet run -f $1 --force &
cd ..
