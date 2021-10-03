Set-Location ..\..\src\MusicStoreService
& dotnet publish -c Release -r linux-x64 -f net6.0;cf push -f manifest.yml -p bin/Release/net6.0/linux-x64/publish

Set-Location ..\OrderService
& dotnet publish -c Release -r linux-x64 -f net6.0;cf push -f manifest.yml -p bin/Release/net6.0/linux-x64/publish

Set-Location ..\ShoppingCartService
& dotnet publish -c Release -r linux-x64 -f net6.0;cf push -f manifest.yml -p bin/Release/net6.0/linux-x64/publish

Set-Location ..\MusicStoreUI
dotnet publish -c Release -r linux-x64 -f net6.0;cf push -f manifest.yml -p bin/Release/net6.0/linux-x64/publish

Set-Location ..\..\deployment\CloudFoundry
