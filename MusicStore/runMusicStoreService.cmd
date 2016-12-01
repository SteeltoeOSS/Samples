cd src\MusicStoreService
dotnet restore --configfile nuget.config
start dotnet run --framework netcoreapp1.1 --server.urls http://*:5000
cd ..\..