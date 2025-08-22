Param(
    [Parameter(Mandatory = $true, HelpMessage = "Escaped UNC path. For example, if the path is '\\localhost\steeltoe_network_share', use '\\\\localhost\\steeltoe_network_share'.")][string]$NetworkAddress,
	[Parameter(Mandatory=$true)][string]$UserName,
	[Parameter(Mandatory=$true)][string]$Password,
    [string]$ServiceName = "credhub",
    [string]$ServicePlan = "default",
    [string]$ServiceInstanceName = "sampleNetworkShare"
)
$ErrorActionPreference = "Stop"

$ParamJSON = [string]::Format('{{\"location\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\"}}', $NetworkAddress, $UserName, $Password)

Write-Host "cf create-service $ServiceName $ServicePlan $ServiceInstanceName -c $ParamJSON -t $ServiceInstanceName"

cf create-service $ServiceName $ServicePlan $ServiceInstanceName -c $ParamJSON -t $ServiceInstanceName
