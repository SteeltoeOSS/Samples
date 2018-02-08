Param([string]$framework)
Start-Process -filepath "dotnet" -argumentlist "run -p .\Fortune-Teller-Service\Fortune-Teller-Service.csproj --force -f $framework"
Start-Process -filepath "dotnet" -argumentlist "run -p .\Fortune-Teller-UI\Fortune-Teller-UI.csproj --force -f $framework"
