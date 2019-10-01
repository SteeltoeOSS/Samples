param (
    [String]
    $projectId = "cf-spring-steeltoe",
    [String]
    $clusterName = "musicstore",
    [String]
    $version = "v1",
    [switch]
    $buildImages = $true,
    [String]
    $deploymentFolder = (Get-Item $PSScriptRoot).Parent
)

Write-Host "Building environment with unique suffix $uid"
$TotalTime = New-Object -TypeName System.Diagnostics.Stopwatch
$TotalTime.Start()

## prerequisites: install gcloud and setup gcloud config
## TODO: bail out if not 


## create resource group, setting default location
$regex = '^'+$clusterName+' '
$cluster_exists = gcloud container clusters list  | Select-String -Pattern $regex
If ($cluster_exists -eq $null)
{
    Write-Host 'Creating Cluster '+$cluster_exists
    gcloud container clusters create $clusterName
}
else
{
    Write-Host 'Cluster already exits: ' + $cluster_exists
}


Write-Host "Setup container registry "
gcloud auth configure-docker

$registry = 'gcr.io/'+$projectId
Write-Host 'Targeting registry: '$registry
if ($buildImages)
{
    ## tag and push containers 
    $images = "musicservice", "orderservice", "shoppingcartservice", "musicstore"

    foreach ($image in $images)
    {
        $tag = $image + ":" + $version
        docker-compose build --parallel $image
        Write-Host "Tagging $image with $registry/$tag"
        docker tag $image $registry/$tag
        Write-Host "Pushing $tag to $registry"
        docker push $registry/$tag
    }
}

Write-Host "Deploying infrastructure services"
kubectl apply -f (Join-Path $deploymentFolder k8s_infra_manifest.yaml)

Write-Host "Replacing tokens in app manifest with env-specific values"
((Get-Content -Path (Join-Path $deploymentFolder k8s_template_apps.yaml) -Raw) `
    -replace '<uid>', $uid `
    -replace '<acr>', $registry `
    -replace '<version>', $version) | `
    Set-Content -Path (Join-Path $PSScriptRoot gcp_apps_manifest.yaml)

Write-Host "Deploying Apps"
kubectl apply -f (Join-Path $PSScriptRoot gcp_apps_manifest.yaml)

$TotalTime.Stop()
Write-Host "Total processing time:" $TotalTime.Elapsed.ToString()