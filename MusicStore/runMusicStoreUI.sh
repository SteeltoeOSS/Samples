#!/bin/bash
cd src/MusicStoreUI
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.0 --server.urls http://0.0.0.0:5555
cd ../..