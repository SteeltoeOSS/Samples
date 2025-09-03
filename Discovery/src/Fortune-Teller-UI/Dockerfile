FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src
COPY  . /src
RUN dotnet publish -f net8.0

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=builder /src/bin/Debug/net8.0/* /app/
ENTRYPOINT [ "dotnet","Fortune-Teller-UI.dll" ]
