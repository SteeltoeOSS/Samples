#!/bin/bash
cd src/ShoppingCartService
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.1
cd ../..