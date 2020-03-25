Set-Location ..\..\src\MusicStoreService
& dotnet publish -c Release -r ubuntu.18.04-x64 -f netcoreapp3.0;cf push -f manifest.yml -p bin/Release/netcoreapp3.0/ubuntu.18.04-x64/publish

Set-Location ..\OrderService
& dotnet publish -c Release -r ubuntu.18.04-x64 -f netcoreapp3.0;cf push -f manifest.yml -p bin/Release/netcoreapp3.0/ubuntu.18.04-x64/publish

Set-Location ..\ShoppingCartService
& dotnet publish -c Release -r ubuntu.18.04-x64 -f netcoreapp3.0;cf push -f manifest.yml -p bin/Release/netcoreapp3.0/ubuntu.18.04-x64/publish

Set-Location ..\MusicStoreUI
dotnet publish -c Release -r ubuntu.18.04-x64 -f netcoreapp3.0;cf push -f manifest.yml -p bin/Release/netcoreapp3.0/ubuntu.18.04-x64/publish

Set-Location ..\..\deployment\CloudFoundry
