cd src\MusicStoreUI
SET BUILD=LOCAL
dotnet restore --configfile nuget.config
start "Music Store UI" dotnet run --framework netcoreapp2.1
cd ..\..