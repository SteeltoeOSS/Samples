FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5555

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Fortune-Teller-UI/Fortune-Teller-UI.csproj", "Fortune-Teller-UI/"]
RUN dotnet restore "Fortune-Teller-UI/Fortune-Teller-UI.csproj"
COPY . .
WORKDIR "/src/Fortune-Teller-UI"
RUN dotnet build "Fortune-Teller-UI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fortune-Teller-UI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fortune-Teller-UI.dll"]