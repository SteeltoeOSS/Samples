#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Basic.csproj", "Basic/"]
RUN dotnet restore "Basic/Basic.csproj"
COPY . Basic
WORKDIR "/src/Basic"
RUN dotnet build "Basic.csproj"  -o /app/build

FROM build AS publish
RUN dotnet publish "Basic.csproj"  -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basic.dll"]