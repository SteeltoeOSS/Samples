cd src\ShoppingCartService
dotnet restore --configfile nuget.config
start "Music Store Cart Service" dotnet run --framework netcoreapp2.0  --server.urls http://*:6000
cd ..\..