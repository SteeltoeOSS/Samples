# Steeltoe All Actuators

## Requirements

- Docker
- [Pack CLI](https://buildpacks.io/docs/tools/pack)

## Local Docker Build

<i>Ensure Docker is running</i>

```bash
pack build steeltoe-actuators --buildpack gcr.io/paketo-buildpacks/dotnet-core --builder paketobuildpacks/builder:base
```

Once Image has been created, run with:

```bash
docker run --rm -p 8080:8080 steeltoe-actuators
```