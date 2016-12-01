@ECHO OFF
IF "%1"=="" GOTO :usage
IF "%2"=="" GOTO :usage
SET r=%1
cd src\OrderService
IF EXIST %CD%\publish (CMD /C "RMDIR /q /s %CD%\publish")
dotnet restore
dotnet publish --output %CD%\publish --configuration Release --runtime %1 --framework %2
IF "%r:~0,3%"=="win" (CMD /c "cf push -f manifest-windows.yml -p %CD%\publish")
IF "%r:~0,6%"=="ubuntu" (CMD /c "cf push -f manifest.yml -p %CD%\publish")
cd ..\..
exit /b
:usage
echo USAGE: 
echo pushOrderService [runtime] [framework]
echo runtime - target runtime to publish (e.g. win7-x64, ubuntu.14.04-x64)
echo framework - target framework to publish (e.g. net451, netcoreapp1.1)
exit /b