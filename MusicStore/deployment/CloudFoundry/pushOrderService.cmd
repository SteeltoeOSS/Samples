@ECHO OFF
IF "%1"=="" GOTO :usage
IF "%2"=="" GOTO :usage
SET r=%1
cd src\OrderService
dotnet restore
dotnet publish --configuration Release --runtime %1 --framework %2
IF "%r:~0,3%"=="win" (CMD /c "cf push -f manifest-windows.yml -p bin\Release\%2\%1\publish")
IF "%r:~0,6%"=="ubuntu" (CMD /c "cf push -f manifest.yml -p bin\Release\%2\%1\publish")
cd ..\..
exit /b
:usage
echo USAGE: 
echo pushOrderService [runtime] [framework]
echo runtime - target runtime to publish (e.g. win10-x64, linux-x64)
echo framework - target framework to publish (e.g. net8.0)
exit /b