FROM microsoft/aspnet:1.0.0-rc1-update1-coreclr
ENV DNX_PACKAGES=/root/.dnx/packages
COPY . /SimpleCloudFoundry/
RUN mkdir -p /SimpleCloudFoundry/app/tmp
COPY cloudfoundry.crt /usr/local/share/ca-certificates/cloudfoundry/cloudfoundry.crt
RUN update-ca-certificates
WORKDIR /SimpleCloudFoundry
RUN ["dnu", "restore", "--runtime", "dnxcore50"]
EXPOSE 5004
ENTRYPOINT ["dnx", "web", "--server.urls", "http://*:5004"]