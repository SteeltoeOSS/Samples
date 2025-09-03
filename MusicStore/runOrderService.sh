#!/bin/bash
cd src/OrderService
dotnet restore --configfile nuget.config
dotnet run --framework net8.0
cd ../..