FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["ShoppingCartService.csproj", ""]
RUN dotnet restore -s "https://api.nuget.org/v3/index.json" -s "https://www.myget.org/F/steeltoedev/api/v3/index.json"
COPY . .
WORKDIR "/src"
RUN dotnet build -c Release -o /app/build -f net6.0

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish -f net6.0

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCartService.dll"]
