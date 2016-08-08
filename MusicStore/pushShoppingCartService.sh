#!/bin/bash
cd src/ShoppingCartService
dotnet restore --configfile nuget.config
dotnet publish --output $PWD/publish --configuration Release --framework net451 --runtime win7-x64
cf push -f manifest-windows.yml -p $PWD/publish
cd ../..