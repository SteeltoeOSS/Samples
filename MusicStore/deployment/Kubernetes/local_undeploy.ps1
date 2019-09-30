$deploymentFolder = (Get-Item $PSScriptRoot).Parent
kubectl delete -f $deploymentFolder\k8s_infra_manifest.yaml
kubectl delete -f $PSScriptRoot\local_apps_manifest.yaml
kubectl delete configmap musicconfig