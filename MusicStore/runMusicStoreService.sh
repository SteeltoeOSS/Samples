#!/bin/bash
cd src/MusicStoreService
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.1
cd ../..