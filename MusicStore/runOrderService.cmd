cd src\OrderService
dotnet restore --configfile nuget.config
start "Music Store Order Service" dotnet run --framework netcoreapp2.1
cd ..\..