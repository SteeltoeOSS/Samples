FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
WORKDIR /src
COPY  . /src
RUN dotnet publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=builder /src/bin/Debug/net6.0/* /app/
ENTRYPOINT [ "dotnet","Fortune-Teller-UI.dll" ]
