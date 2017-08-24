cd src\MusicStoreUI
dotnet restore --configfile nuget.config
start dotnet run --framework netcoreapp1.1 --server.urls http://*:5555
cd ..\..