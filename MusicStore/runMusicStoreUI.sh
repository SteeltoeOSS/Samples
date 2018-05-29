#!/bin/bash
cd src/MusicStoreUI
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.1
cd ../..