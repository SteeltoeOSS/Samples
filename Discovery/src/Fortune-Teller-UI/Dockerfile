FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /src
COPY  . /src
RUN dotnet publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=builder /src/bin/Debug/net6.0/* /app/
ENTRYPOINT [ "dotnet","Fortune-Teller-UI.dll" ]
