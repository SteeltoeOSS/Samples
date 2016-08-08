cd src\OrderService
dotnet restore --configfile nuget.config
start dotnet run --framework net451 --server.urls http://*:7000
cd ..\..