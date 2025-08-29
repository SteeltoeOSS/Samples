#!/bin/bash
mkdir -p ~/steeltoe/config-repo
docker run -p 8888:8888 -v ~/steeltoe/config-repo:/steeltoe/config-repo --rm -it  steeltoe.azurecr.io/config-server