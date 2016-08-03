cd src\MusicStoreUI
dotnet restore
dotnet publish --output %CD%\publish --configuration Release --runtime win7-x64
cf push -f manifest-windows.yml -p %CD%\publish
cd ..\..