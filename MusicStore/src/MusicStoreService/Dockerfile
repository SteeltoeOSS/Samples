FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["MusicStoreService.csproj", ""]
COPY ["git.properties", ""]
RUN dotnet restore -s "https://api.nuget.org/v3/index.json" -s "https://www.myget.org/F/steeltoedev/api/v3/index.json"
COPY . .
WORKDIR "/src"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicStoreService.dll"]
