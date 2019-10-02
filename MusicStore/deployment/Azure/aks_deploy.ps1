param (
    [String]    
    $uid = "112",
    [String]
    $version = "v1",
    [String]
    $location = "eastus2",
    [String]
    $rg = "mstore$uid",
    [String]
    $cluster = "mcluster$uid",
    [String]
    $registryId = "mregistry$uid",
    [switch]
    $skipBuildImages = $false,
    [String]
    $deploymentFolder = (Get-Item $PSScriptRoot).Parent
)

Write-Host "Building environment with unique suffix $uid"
$TotalTime = New-Object -TypeName System.Diagnostics.Stopwatch
$TotalTime.Start()

## login is not extremely straightforward. Sign in before running this script - ref: https://docs.microsoft.com/en-us/cli/azure/authenticate-azure-cli?view=azure-cli-latest
## TODO: bail out if not logged in

## select subscription
# az account set --subscription <subscription-id>

## create resource group, setting default location
Write-Host "Creating resource group $rg in $location"
az group create --name $rg -l $location

Write-Host "Creating container registry $registryId"
az acr create --name $registryId --resource-group $rg --sku basic --admin-enabled true
Write-Host "Logging in to container registry"
az acr login --name $registryId
$acrLoginServer = az acr show --name $registryId --query loginServer --output tsv

Write-Host "Creating a service principal for AKS cluster"
$servicePrincipal = az ad sp create-for-rbac --skip-assignment --name "http://$cluster"
$spAppId = $servicePrincipal | jq .appId
$spPassword = $servicePrincipal | jq .password
$acrId = az acr show --name $registryId --query id --output tsv
Write-Host "Wait 10 seconds for the service principal to settle in..."
Start-Sleep -s 10

#Write-Host "Granting the service principal permission to pull from the container registry"
#az role assignment create --assignee $spAppId --scope $acrId --role acrpull

$AKSTime = New-Object -TypeName System.Diagnostics.Stopwatch
$AKSTime.Start()
Write-Host "Kicking off AKS deployment"

az aks create `
    -g $rg --name $cluster --service-principal $spAppId --client-secret $spPassword --attach-acr $acrId `
    --node-count 2 --node-vm-size Standard_D2s_v3 --dns-name-prefix mstore$uid `
    --generate-ssh-keys --enable-addons http_application_routing --debug --no-wait

Write-Host "Time to issue provisioning command for AKS cluster:" $AKSTime.Elapsed.ToString()

if (!$skipBuildImages)
{
    Write-Host "Building and pushing app images"
    ## tag and push containers 
    $images = "musicservice", "orderservice", "shoppingcartservice", "musicstore"
    foreach ($image in $images)
    {
        $tag = $image + ":" + $version
        docker-compose build --parallel $image
        Write-Debug "Tagging $image with $acrLoginServer/$tag"
        docker tag $image $acrLoginServer/$tag
        Write-Host "Pushing $tag to $acrLoginServer"
        docker push $acrLoginServer/$tag
    }
}

Write-Host "Waiting for AKS provisioning to complete..."
az aks wait -g $rg --name $cluster --created
$AKSTime.Stop()
Write-Host "Time to provision AKS cluster:" $AKSTime.Elapsed.ToString()

Write-Host "Pointing kubectl at the new AKS cluster"
az aks get-credentials -g $rg --name $cluster --overwrite-existing

Write-Host "Creating configmap with contents of file located at" (Join-Path $deploymentFolder Kubernetes musicconfig.yaml)
kubectl apply -f (Join-Path $deploymentFolder Kubernetes musicconfig.yaml)

Write-Host "Deploying infrastructure services"
kubectl apply -f (Join-Path $deploymentFolder k8s_infra_manifest.yaml)

Write-Host "Replacing tokens in app manifest with env-specific values"
((Get-Content -Path (Join-Path $deploymentFolder k8s_template_apps.yaml) -Raw) `
    -replace '<uid>', $uid `
    -replace '<acr>', $acrLoginServer `
    -replace '<version>', $version) | `
    Set-Content -Path (Join-Path $PSScriptRoot aks_apps_manifest.yaml)

Write-Host "Deploying Apps"
kubectl apply -f (Join-Path $PSScriptRoot aks_apps_manifest.yaml)

$dnsZone = az aks show -g $rg -n $cluster --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName -o tsv
Write-Host "Replacing tokens in ingress manifest with env-specific values"
((Get-Content -Path (Join-Path $PSScriptRoot aks_ingress_template.yaml) -Raw) `
    -replace '<CLUSTER_SPECIFIC_DNS_ZONE>', $dnsZone) | `
    Set-Content -Path (Join-Path $PSScriptRoot aks_ingress_manifest.yaml)
kubectl apply -f (Join-Path $PSScriptRoot aks_ingress_manifest.yaml)

Write-Host "Deploying Apps"
kubectl apply -f (Join-Path $PSScriptRoot aks_apps_manifest.yaml)

$TotalTime.Stop()
Write-Host "Total processing time:" $TotalTime.Elapsed.ToString()