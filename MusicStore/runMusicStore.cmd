SET BUILD=LOCAL

dotnet restore
dotnet build --no-restore

cd src\MusicStoreService
start "Music Store Service" dotnet run --no-build
cd ..\..

cd src\MusicStoreUI
start "Music Store UI" dotnet run --no-build
cd ..\..

cd src\OrderService
start "Music Store Order Service" dotnet run --no-build
cd ..\..

cd src\ShoppingCartService
start "Music Store Cart Service" dotnet run --no-build
cd ..\..

explorer "http://localhost:5555"
