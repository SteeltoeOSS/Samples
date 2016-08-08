#!/bin/bash
cd src/MusicStoreUI
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp1.0 --server.urls http://*:5555
cd ../..