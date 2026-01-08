Param(
    [Parameter(Mandatory = $true, HelpMessage = "UNC path to the network share. For example: '\\localhost\steeltoe_network_share'")][string]$NetworkAddress,
	[Parameter(Mandatory=$true)][string]$UserName,
	[Parameter(Mandatory=$true)][string]$Password,
    [string]$ServiceName = "credhub",
    [string]$ServicePlan = "default",
    [string]$ServiceInstanceName = "sampleNetworkShare"
)
$ErrorActionPreference = "Stop"

# Escape backslashes for JSON
$EscapedNetworkAddress = $NetworkAddress -replace '\\', '\\'

$ParamJSON = [string]::Format('{{\"location\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\"}}', $EscapedNetworkAddress, $UserName, $Password)

Write-Host "cf create-service $ServiceName $ServicePlan $ServiceInstanceName -c $ParamJSON -t $ServiceInstanceName"

cf create-service $ServiceName $ServicePlan $ServiceInstanceName -c $ParamJSON -t $ServiceInstanceName
