FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster as builder
WORKDIR /src
COPY ["Fortune-Teller-UI/FortuneTeller.UI.csproj", "Fortune-Teller-UI/"]
RUN dotnet restore "Fortune-Teller-UI/"
COPY . .
RUN dotnet publish "Fortune-Teller-UI/" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "FortuneTeller.UI.dll"]