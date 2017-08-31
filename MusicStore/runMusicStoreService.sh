#!/bin/bash
cd src/MusicStoreService
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.0 --server.urls http://*:5000
cd ../..