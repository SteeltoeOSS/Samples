$uid = "123"
$version = 1
$rg = "musicstore$uid"
$registryId = "musicregistry$uid"
$configId = "musicconfig$uid"

# remove container instances
$containers = "musicstore", "musicservice", "orderservice", "shoppingcartservice", "sqlserver"
foreach ($c in $containers)
{
    az container delete --name $c -g $rg -y
}

# remove images from registry
foreach ($c in $containers)
{
    $vloop = $version
    while ($vloop -gt 0)
    {
        $tag = $c + ":v" + $vloop
        if ($c -ne "sqlserver"){
            az acr repository delete --name $registryId --image $tag -y
        }
        $vloop--
    }
}

az appconfig delete -g $rg -n $configId