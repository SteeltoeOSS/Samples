FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", ""]
COPY ["/OrderService.csproj", ""]
RUN dotnet restore -s "https://api.nuget.org/v3/index.json" -s "https://pkgs.dev.azure.com/dotnet/Steeltoe/_packaging/dev/nuget/v3/index.json"
COPY . .
WORKDIR "/src"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]
