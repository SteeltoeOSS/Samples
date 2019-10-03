param (
    [String]    
    $uid = "110",
    [String]
    $version = "v1",
    [String]
    $location = "us-east-2",
    [String]
    $cluster = "mcluster$uid",
    [switch]
    $skipBuildImages = $false,
    [String]
    $deploymentFolder = (Get-Item $PSScriptRoot).Parent
)

Write-Host "Building environment with unique suffix $uid"
$TotalTime = New-Object -TypeName System.Diagnostics.Stopwatch
$TotalTime.Start()

# Pre-Reqs: 
# - install AKS CLI, eksctl, aws-iam-authenticator
# - run 'aws configure'

## TODO: bail out if not logged in
 $EKSTime = New-Object -TypeName System.Diagnostics.Stopwatch
 $EKSTime.Start()

 Write-Host "Kicking off EKS deployment"
 eksctl create cluster --name $cluster --region $location --zones $location"a",$location"b",$location"c" --version 1.14 `
     --nodegroup-name $cluster-workers  --node-type t3.medium `
     --nodes-min 1 --nodes-max 4 --node-ami auto
 $EKSTime.Stop()
 Write-Host "Time to provision EKS cluster:" $EKSTime.Elapsed.ToString()

$registryUri = "aws"
if (!$skipBuildImages)
{
    Write-Host "Building and pushing app images"
    ## tag and push containers 
    $images = "musicservice", "orderservice", "shoppingcartservice", "musicstore"
    $existingRepositories = aws ecr describe-repositories --region $location
    aws ecr get-login --no-include-email --region $location | Invoke-Expression 

    foreach ($image in $images)
    {
        $registryUri = $existingRepositories | jq ".repositories[] | select(.repositoryName==\""$image\"").repositoryUri" -r
        if (!$registryUri)
        {
            Write-Host "Repo not detected, attempting to create"
            $registryUri = aws ecr create-repository --repository-name $image --region $location | jq .repository.repositoryUri -r
        }
        Write-Host "RegistryUri = $registryUri"
        $fullpath = $registryUri + ":" + $version
        docker-compose build --parallel $image
        Write-Host "docker tag $image $fullpath"
        docker tag $image $fullpath
        Write-Host "docker push $fullpath"
        docker push $fullpath
    }
}

Write-Host "Creating configmap with contents of file located at" (Join-Path $deploymentFolder Kubernetes musicconfig.yaml)
kubectl apply -f (Join-Path $deploymentFolder Kubernetes musicconfig.yaml)

Write-Host "Deploying infrastructure services"
kubectl apply -f (Join-Path $deploymentFolder k8s_infra_manifest.yaml)

Write-Host "Replacing tokens in app manifest with env-specific values"
((Get-Content -Path (Join-Path $deploymentFolder k8s_template_apps.yaml) -Raw) `
    -replace '<uid>', $uid `
    -replace '<acr>', $registryUri.Replace("/musicstore", "") `
    -replace '<version>', $version) | `
    Set-Content -Path (Join-Path $PSScriptRoot aws_apps_manifest.yaml)

Write-Host "Deploying Apps"
kubectl apply -f (Join-Path $PSScriptRoot aws_apps_manifest.yaml)

$TotalTime.Stop()
Write-Host "Total processing time:" $TotalTime.Elapsed.ToString()