@ECHO OFF
IF "%1"=="" GOTO :usage
start "Fortune Teller Service" dotnet run -p .\Fortune-Teller-Service\Fortune-Teller-Service.csproj --force -f %1
SET BUILD=LOCAL
start "Fortune Teller UI" dotnet run -p .\Fortune-Teller-UI\Fortune-Teller-UI.csproj --force -f %1
SET BUILD=
:usage
echo USAGE: 
echo RunFortuneTeller [framework]
echo framework - target framework to publish (e.g. net461, netcoreapp2.1)
exit /b