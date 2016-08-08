cd src\MusicStoreService
dotnet restore --configfile nuget.config
start dotnet run --framework net451 --server.urls http://*:5000
cd ..\..