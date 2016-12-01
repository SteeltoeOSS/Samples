cd src\ShoppingCartService
dotnet restore --configfile nuget.config
start dotnet run --framework netcoreapp1.1  --server.urls http://*:6000
cd ..\..