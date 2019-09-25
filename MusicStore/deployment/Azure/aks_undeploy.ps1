$uid = "123"
$version = 1
$rg = "musicstore$uid"
$cluster = "musiccluster$uid"
$registryId = "musicregistry$uid"

Write-Host "Deleting cluster (with --no-wait)"
az aks delete --name $cluster --resource-group $rg -y --no-wait

# remove images from registry
# $containers = "musicstore", "musicservice", "orderservice", "shoppingcartservice"
# foreach ($c in $containers)
# {
#     $vloop = $version
#     while ($vloop -gt 0)
#     {
#         $tag = $c + ":v" + $vloop
#         Write-Host "Deleting image for $tag"
#         az acr repository delete --name $registryId --image $tag -y
#         $vloop--
#     }
# }

Write-Host "Deleting service principal created for AKS cluster"
az ad sp delete --id "http://$cluster"