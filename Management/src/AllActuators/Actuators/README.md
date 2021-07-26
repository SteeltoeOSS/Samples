# Steeltoe All Actuators

This sample provides a list of all actuator endpoints available through Steeltoe

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