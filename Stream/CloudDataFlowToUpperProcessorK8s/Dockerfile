#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM base AS final
WORKDIR /app
COPY bin/Debug/net5.0/publish .
EXPOSE 8080
ENV PORT 8080
ENTRYPOINT ["dotnet", "CloudDataflowToUpperProcessor.dll"]
