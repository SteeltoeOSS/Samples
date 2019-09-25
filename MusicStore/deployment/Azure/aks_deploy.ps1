# add params for location, subscription id
# create short random id to use for all things that need unique routes
$uid = "123"
$version = "v1"
$location = "eastus2"
$rg = "musicstore$uid"
$cluster = "musiccluster$uid"
$registryId = "musicregistry$uid"
$buildImages = $false
$deploymentFolder = (Get-Item $PSScriptRoot).Parent

Write-Host "Building environment with unique suffix $uid"
$TotalTime = New-Object -TypeName System.Diagnostics.Stopwatch
$TotalTime.Start()

## login is not extremely straightforward. Sign in before running this script - ref: https://docs.microsoft.com/en-us/cli/azure/authenticate-azure-cli?view=azure-cli-latest
## TODO: bail out if not logged in

## select subscription
# az account set --subscription <subscription-id>

## create resource group, setting default location
Write-Host "Creating resource group $rg in $location"
# az group create --name $rg -l $location

Write-Host "Creating container registry $registryId"
# az acr create --name $registryId --resource-group $rg --sku basic --admin-enabled true
Write-Host "Logging in to container registry"
az acr login --name $registryId
$acrLoginServer = az acr show --name $registryId --query loginServer --output tsv

Write-Host "Creating a service principal for AKS cluster"
$servicePrincipal = az ad sp create-for-rbac --skip-assignment --name "http://$cluster"
$spAppId = $servicePrincipal | jq .appId
$spPassword = $servicePrincipal | jq .password
$acrId = az acr show --name $registryId --query id --output tsv
Write-Host "Wait 10 seconds for the service principal to settle in before adding permissions..."
Start-Sleep -s 10
Write-Host "Granting the service principal permission to pull from the container registry"
az role assignment create --assignee $spAppId --scope $acrId --role acrpull

$AKSTime = New-Object -TypeName System.Diagnostics.Stopwatch
$AKSTime.Start()
Write-Host "Deploying AKS cluster... this might take some time"
az aks create -g $rg --name $cluster --service-principal $spAppId --client-secret $spPassword --node-count 1 --generate-ssh-keys --enable-addons http_application_routing --verbose
$AKSTime.Stop()
Write-Host "Time to provision AKS cluster:" $TotalTime.Elapsed.ToString()

Write-Host "Pointing kubectl at the new AKS cluster"
az aks get-credentials --resource-group $rg --name $cluster --overwrite-existing

kubectl create configmap musicconfig --from-file=$PSScriptRoot/musicconfig.yaml

# TODO: Use Azure SQL with Managed Identity access instead
Write-Host "Deploying SQL Server"
kubectl apply -f $deploymentFolder\k8s_infra_manifest.yaml

if ($buildImages)
{
    ## tag and push containers 
    $images = "musicservice", "orderservice", "shoppingcartservice", "musicstore"
    foreach ($image in $images)
    {
        $tag = $image + ":" + $version
        docker-compose build $image
        Write-Debug "Tagging $image with $acrLoginServer/$tag"
        docker tag $image $acrLoginServer/$tag
        Write-Host "Pushing $tag to $acrLoginServer"
        docker push $acrLoginServer/$tag
    }
}

Write-Host "Replacing tokens in app manifest with env-specific values"
((Get-Content -Path $deploymentFolder\k8s_template_apps.yaml -Raw) `
    -replace '<uid>', $uid `
    -replace '<acr>', $acrLoginServer `
    -replace '<version>', $version) | `
    Set-Content -Path $PSScriptRoot\aks_apps_manifest.yaml

Write-Host "Deploying Apps"
kubectl apply -f $PSScriptRoot\aks_apps_manifest.yaml

$TotalTime.Stop()
Write-Host "Total processing time:" $TotalTime.Elapsed.ToString()