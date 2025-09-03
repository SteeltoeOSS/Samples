#!/bin/bash
cd src/ShoppingCartService
dotnet restore --configfile nuget.config
dotnet run --framework net8.0
cd ../..