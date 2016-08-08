#!/bin/bash
docker run -p 5432:5432 --rm -it -e POSTGRES_DB=steeltoe -e POSTGRES_USER=steeltoe -e POSTGRES_PASSWORD=steeltoe  postgres:9.5.3 