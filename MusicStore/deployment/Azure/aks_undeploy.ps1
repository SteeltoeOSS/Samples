param
(
    [String]    
    $uid = "112",
    [String]
    $version = 1,
    [String]
    $rg = "mstore$uid",
    [String]
    $cluster = "mcluster$uid",
    [String]
    $registryId = "mregistry$uid",
    [switch]
    $cleanImages = $false,
    [String]
    $originalKubeContext = "docker-desktop"
)

Write-Host "Deleting cluster (with --no-wait)"
az aks delete --name $cluster --resource-group $rg -y --no-wait

if ($cleanImages)
{
    $containers = "musicstore", "musicservice", "orderservice", "shoppingcartservice"
    # remove images from registry
    foreach ($c in $containers)
    {
        $vloop = $version
        while ($vloop -gt 0)
        {
            $tag = $c + ":v" + $vloop
            az acr repository delete --name $registryId --image $tag -y
            $vloop--
        }
    }
}

Write-Host "Deleting service principal created for AKS cluster"
az ad sp delete --id "http://$cluster"

kubectl config set current-context $originalKubeContext
Write-Host "Removing demo Kubernetes context $cluster"
kubectl config delete-context $cluster
Write-Host "kubectl config current-context =" (kubectl config current-context)