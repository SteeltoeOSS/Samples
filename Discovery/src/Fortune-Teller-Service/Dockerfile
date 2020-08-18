FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
WORKDIR /src
COPY  . /src
RUN dotnet publish -f netcoreapp3.1

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=builder /src/bin/Debug/netcoreapp3.1/* /app/
ENTRYPOINT [ "dotnet","Fortune-Teller-Service.dll" ]
