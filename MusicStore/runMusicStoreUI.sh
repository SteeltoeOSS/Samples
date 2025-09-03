#!/bin/bash
cd src/MusicStoreUI
dotnet restore --configfile nuget.config
dotnet run --framework net8.0
cd ../..