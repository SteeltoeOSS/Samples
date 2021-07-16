# Steeltoe All Actuators

This sample is available in Docker Hub - [steeltoemain/steeltoe-actuators](https://hub.docker.com/repository/docker/steeltoemain/steeltoe-actuators)


## Local Docker Build

### Requirements

- Docker (running)
- [Pack CLI](https://buildpacks.io/docs/tools/pack)

```bash
pack build steeltoe-actuators --buildpack gcr.io/paketo-buildpacks/dotnet-core --builder paketobuildpacks/builder:base
```

Once Image has been created, run with:

```bash
docker run --rm -p 8080:8080 steeltoe-actuators
```