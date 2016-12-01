cd src\OrderService
dotnet restore --configfile nuget.config
start dotnet run --framework netcoreapp1.1 --server.urls http://*:7000
cd ..\..