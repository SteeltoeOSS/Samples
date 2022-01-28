FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as builder
WORKDIR /src
COPY ["Fortune-Teller-Service/FortuneTeller.Service.csproj", "Fortune-Teller-Service/"]
RUN dotnet restore "Fortune-Teller-Service/"
COPY . .
RUN dotnet publish "Fortune-Teller-Service/" -c Release -o /app -f net5.0

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "FortuneTeller.Service.dll"]