#!/bin/bash
cd src/ShoppingCartService
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.0 --server.urls http://0.0.0.0:6000
cd ../..