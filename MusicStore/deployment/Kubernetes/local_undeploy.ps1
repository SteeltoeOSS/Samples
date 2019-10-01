$deploymentFolder = (Get-Item $PSScriptRoot).Parent
kubectl delete -f (Join-Path $deploymentFolder k8s_infra_manifest.yaml)
kubectl delete -f (Join-Path $PSScriptRoot local_apps_manifest.yaml)
kubectl delete configmap musicconfig