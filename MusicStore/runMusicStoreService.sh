#!/bin/bash
cd src/MusicStoreService
dotnet restore --configfile nuget.config
dotnet run --framework net8.0
cd ../..