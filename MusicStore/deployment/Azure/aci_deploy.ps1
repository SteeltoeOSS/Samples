# add params for location, subscription id
# create short random id to use for all things that need unique routes
$uid = "123"
$version = "v1"
$location = "eastus2"

Write-Host "Building environment with unique suffix $uid"

## login is not extremely straightforward. Sign in before running this script
## ref: https://docs.microsoft.com/en-us/cli/azure/authenticate-azure-cli?view=azure-cli-latest
## TODO: bail out if not logged in

## select subscription
# az account set --subscription <subscription-id>

## create resource group, setting default location
$rg = "musicstore$uid"
Write-Host "Creating resource group $rg"
az group create -l $location --name $rg

## create azure container registry
$registryId = "musicregistry$uid"
Write-Host "Creating container registry $registryId"
# az acr create --name $registryId --resource-group $rg --sku basic --admin-enabled true
Write-Host "Fetching registry credentials"
$registryPassword = az acr credential show -n $registryId | jq ".passwords[0].value" -r

## build containers
## locate docker-compose.yml
Write-Host "Logging in to container registry"
az acr login --name $registryId
$acrLoginServer = az acr show --name $registryId --query loginServer

# deploy the sql server well in advance so it can spin up fully
Write-Host "Deploying SQL Server"
az container create --image mcr.microsoft.com/mssql/server -g musicstore123 -n sqlserver --cpu 3 --memory 3.5  --no-wait `
     --dns-name-label="musicsql$uid" --ports 1433 `
     --environment-variables ACCEPT_EULA=Y SA_PASSWORD=SteeltoeR0cks!

# TODO: Create AppConfig, import settings into it, set permissions (container instances need to be Contributor)

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
    Write-Host("Creating container $image from $acrLoginServer/$tag in $rg")
    az container create -g $rg --name $image --image $acrLoginServer/$tag --registry-login-server $acrLoginServer --registry-username $registryId --registry-password $registryPassword --no-wait `
        --dns-name-label=$image$uid --ports 80 --assign-identity `
        --environment-variables ASPNETCORE_ENVIRONMENT=AzureContainerInstances sqlserver__credentials__server="musicsql$uid.$location.azurecontainer.io" AppConfig__Endpoint="https://musicconfig$uid.azconfig.io"
}
