FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -f net6.0 -c release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .

ENV SPRING_RABBITMQ_HOST=host.docker.internal
ENV PORT=8080

ENTRYPOINT ["dotnet", "UsageSender.dll"]