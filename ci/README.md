# SteeltoeOSS Concourse Pipeline

**Login**
```
$ fly --target steeltoe login --team-name steeltoe -concourse-url https://ci.spring.io
```

**Update the Pipeline**

_Note: you may need to run `lpass login` prior to updating the pipeline._

```
$ fly --target steeltoe set-pipeline --pipeline steeltoe-samples --config ci/pipeline.yml --load-vars-from <(lpass show --notes 'Shared-Steeltoe/concourse.yml')
```
