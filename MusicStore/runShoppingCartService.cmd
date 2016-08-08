cd src\ShoppingCartService
dotnet restore --configfile nuget.config
start dotnet run --framework net451  --server.urls http://*:6000
cd ..\..