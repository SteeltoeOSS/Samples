FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src
COPY ["Fortune-Teller-UI/FortuneTeller.UI.csproj", "Fortune-Teller-UI/"]
COPY ["Fortune-Teller-UI/Directory.Build.props", "Fortune-Teller-UI/"]
COPY ["Fortune-Teller-UI/nuget.config", "Fortune-Teller-UI/"]
RUN dotnet restore "Fortune-Teller-UI/"
COPY . .
RUN dotnet publish "Fortune-Teller-UI/" -c Release -o /app -f net8.0

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "FortuneTeller.UI.dll"]