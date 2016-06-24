FROM microsoft/dotnet
ENV NUGET_PACKAGES=/root/.nuget/packages
COPY . /SimpleCloudFoundry/
RUN mkdir -p /SimpleCloudFoundry/app/tmp
WORKDIR /SimpleCloudFoundry
RUN ["dotnet", "restore"]
EXPOSE 5004
ENTRYPOINT ["/usr/bin/env", "TMPDIR=/tmp", "dotnet", "run", "--server.urls", "http://*:5004"]
