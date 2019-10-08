param (
    [String]
    $projectId = "cf-spring-steeltoe"
)
$deploymentFolder = (Get-Item $PSScriptRoot).Parent
kubectl delete -f (Join-Path $deploymentFolder k8s_infra_manifest.yaml)
kubectl delete -f (Join-Path $PSScriptRoot gcp_apps_manifest.yaml)
kubectl delete configmap musicconfig
$containers = "musicstore", "musicservice", "orderservice", "shoppingcartservice", "sqlserver", "eurekaserver", "adminserver"

foreach ($c in $containers)
{
    gcloud container images delete "gcr.io/$projectId/$c" --force-delete-tags
}