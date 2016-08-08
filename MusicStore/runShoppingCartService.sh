#!/bin/bash
cd src/ShoppingCartService
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp1.0 --server.urls http://*:6000
cd ../..