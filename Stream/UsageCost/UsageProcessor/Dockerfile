FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -f net5.0 -c release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENV SPRING_RABBITMQ_HOST=host.docker.internal

ENTRYPOINT ["dotnet", "UsageProcessor.dll"]