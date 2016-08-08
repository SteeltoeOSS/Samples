cd src\MusicStoreUI
dotnet restore --configfile nuget.config
start dotnet run --framework net451 --server.urls http://*:5555
cd ..\..