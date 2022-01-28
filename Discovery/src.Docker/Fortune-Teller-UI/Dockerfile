FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as builder
WORKDIR /src
COPY ["Fortune-Teller-UI/FortuneTeller.UI.csproj", "Fortune-Teller-UI/"]
RUN dotnet restore "Fortune-Teller-UI/"
COPY . .
RUN dotnet publish "Fortune-Teller-UI/" -c Release -o /app -f net5.0

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "FortuneTeller.UI.dll"]