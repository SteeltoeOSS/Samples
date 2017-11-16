FROM ubuntu:16.04
ARG DEBIAN_FRONTEND=noninteractive

# base
RUN apt-get update && apt-get -y upgrade
RUN apt-get -y install apt-utils
RUN apt-get -y install apt-transport-https
RUN apt-get -y install curl
RUN apt-get -y install gcc
RUN apt-get -y install git
RUN apt-get -y install wget

# DotNet
RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /etc/apt/trusted.gpg.d/microsoft.gpg
RUN echo "deb https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main" > /etc/apt/sources.list.d/dotnetdev.list
RUN apt-get update
RUN apt-get -y install dotnet-sdk-2.0.0

# CloudFoundry
RUN curl https://packages.cloudfoundry.org/debian/cli.cloudfoundry.org.key | apt-key add -
RUN echo "deb http://packages.cloudfoundry.org/debian stable main" > /etc/apt/sources.list.d/cloudfoundry-cli.list
RUN apt-get update
RUN apt-get -y install cf-cli

# Java
RUN apt-get -y install openjdk-8-jdk
RUN apt-get -y install maven

# Behave et al
RUN apt-get -y install python3
RUN apt-get -y install python3-pip
RUN pip3 install --upgrade pip
RUN mkdir /etc/SteeltoeOSS
RUN wget https://raw.githubusercontent.com/SteeltoeOSS/Samples/dev/pyenv.pkgs \
    -O /etc/SteeltoeOSS/pyenv.pkgs
RUN pip3 install --requirement /etc/SteeltoeOSS/pyenv.pkgs
