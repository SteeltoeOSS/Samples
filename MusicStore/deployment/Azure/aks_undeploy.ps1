$uid = "123"
$version = 1
$rg = "musicstore$uid"
$cluster = "musiccluster$uid"
$registryId = "musicregistry$uid"
$cleanImages = $false

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