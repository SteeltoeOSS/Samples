FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster as builder
WORKDIR /src
COPY ["Fortune-Teller-Service/FortuneTeller.Service.csproj", "Fortune-Teller-Service/"]
RUN dotnet restore "Fortune-Teller-Service/"
COPY . .
RUN dotnet publish "Fortune-Teller-Service/" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "FortuneTeller.Service.dll"]