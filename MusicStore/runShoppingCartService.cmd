cd src\ShoppingCartService
dotnet restore --configfile nuget.config
start "Music Store Cart Service" dotnet run --framework netcoreapp2.1
cd ..\..