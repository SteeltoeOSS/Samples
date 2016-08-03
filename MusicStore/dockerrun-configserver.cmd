mkdir \steeltoe\config-repo
docker run -p 8888:8888 -v c:/steeltoe/config-repo:/steeltoe/config-repo --rm -it dtillman/steeltoe:configserver-linux