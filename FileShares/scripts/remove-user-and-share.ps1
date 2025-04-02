Param(
    [string]$ShareName = "steeltoe_network_share",
    [string]$SharePath = "c:\steeltoe_network_share",
    [string]$UserName = "shareWriteUser"
)
$ErrorActionPreference = "Stop"
if ($PSVersionTable.PSVersion.Major -lt 6)
{
    Write-Output "Running in Windows PowerShell (version < 6)"
}
else
{
    Write-Output "Running in PowerShell (Pwsh) 7+"
    Add-Type -AssemblyName System.Management.Automation
    Import-Module Microsoft.PowerShell.LocalAccounts -SkipEditionCheck
}
#Requires -RunAsAdministrator
#Requires -Modules Microsoft.PowerShell.LocalAccounts, SmbShare

if (Get-SmbShare $ShareName -ErrorAction SilentlyContinue)
{
    Remove-SmbShare -Name $ShareName
    Write-Host "SMB share $ShareName removed."
}
else
{
    Write-Host "SMB share $ShareName was not found."
}

if (Get-LocalUser -Name $UserName -ErrorAction SilentlyContinue)
{
    if (Get-LocalGroupMember -Group "Users" -Member $UserName -ErrorAction SilentlyContinue)
    {
        Write-Host "Removing $UserName from local 'Users' group..."
        Remove-LocalGroupMember -Group "Users" -Member $UserName
        Write-Host "User removed from group."
    }
    else
    {
        Write-Host "User $UserName was not found in 'Users' group."
    }
    Write-Host "Removing local user $UserName..."
    Remove-LocalUser -Name $UserName
    Write-Host "User completely removed."
}
else
{
    Write-Host "User $UserName was not found."
}

if (Get-Item -Path $SharePath -ErrorAction SilentlyContinue)
{
    Write-Host "Removing $SharePath from disk..."
    Remove-Item -Path $SharePath -Recurse
    Write-Host "Directory completely removed."
}
else
{
    Write-Host "$SharePath was not found."
}
