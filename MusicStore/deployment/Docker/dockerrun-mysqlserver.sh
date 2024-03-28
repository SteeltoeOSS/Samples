#!/bin/bash
docker run -p 3306:3306 --rm -it -e MYSQL_ROOT_PASSWORD=steeltoe -e MYSQL_DATABASE=steeltoe -e MYSQL_USER=steeltoe -e MYSQL_PASSWORD=steeltoe  mysql:5.7.13 --console