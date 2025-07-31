FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src
COPY ["Fortune-Teller-Service/FortuneTeller.Service.csproj", "Fortune-Teller-Service/"]
COPY ["Fortune-Teller-Service/Directory.Build.props", "Fortune-Teller-Service/"]
COPY ["Fortune-Teller-Service/nuget.config", "Fortune-Teller-Service/"]
RUN dotnet restore "Fortune-Teller-Service/"
COPY . .
RUN dotnet publish "Fortune-Teller-Service/" -c Release -o /app -f net8.0

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "FortuneTeller.Service.dll"]