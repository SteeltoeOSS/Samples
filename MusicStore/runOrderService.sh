#!/bin/bash
cd src/OrderService
dotnet restore --configfile nuget.config
dotnet run --framework netcoreapp2.1
cd ../..