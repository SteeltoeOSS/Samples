FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Fortune-Teller-Service/Fortune-Teller-Service.csproj", "Fortune-Teller-Service/"]
RUN dotnet restore "Fortune-Teller-Service/Fortune-Teller-Service.csproj"
COPY . .
WORKDIR "/src/Fortune-Teller-Service"
RUN dotnet build "Fortune-Teller-Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fortune-Teller-Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fortune-Teller-Service.dll"]