#Requires -Version 7.0

Param(
    [Parameter(Mandatory = $true, HelpMessage = "UNC path to the network share. For example: '\\localhost\steeltoe_network_share'")][string]$NetworkAddress,
    [Parameter(Mandatory = $true)][string]$UserName,
    [Parameter(Mandatory = $true)][string]$Password,
    [Parameter(Mandatory = $false)][string]$ServiceName = "credhub",
    [Parameter(Mandatory = $false)][string]$ServicePlan = "default",
    [Parameter(Mandatory = $false)][string]$ServiceInstanceName = "sampleNetworkShare"
)
$ErrorActionPreference = "Stop"

# Build parameter object and convert to JSON using PowerShell's built-in JSON serialization
# This automatically handles escaping of special characters including backslashes, quotes, etc.
$params = @{
    location = $NetworkAddress
    username = $UserName
    password = $Password
}
$jsonParams = $params | ConvertTo-Json -Compress

# Create a redacted copy of the parameters for logging so the password is not exposed
$redactedParams = $params.Clone()
$redactedParams['password'] = 'REDACTED'
$redactedJsonParams = $redactedParams | ConvertTo-Json -Compress

Write-Host "cf create-service $ServiceName $ServicePlan $ServiceInstanceName -c $redactedJsonParams -t $ServiceInstanceName"
cf create-service $ServiceName $ServicePlan $ServiceInstanceName -c $jsonParams -t $ServiceInstanceName
