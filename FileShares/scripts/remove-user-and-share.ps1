Param(
    [string]$ShareName = "steeltoe_network_share",
    [string]$FolderPath = "c:\steeltoe_network_share",
    [string]$UserName = "shareWriteUser"
)
$ErrorActionPreference = "Stop"
#Requires -RunAsAdministrator
#Requires -Modules Microsoft.PowerShell.LocalAccounts, SmbShare
Import-Module Microsoft.PowerShell.LocalAccounts -SkipEditionCheck

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

if (Get-Item -Path $FolderPath -ErrorAction SilentlyContinue)
{
    Write-Host "Removing $FolderPath from disk..."
    Remove-Item -Path $FolderPath -Recurse
    Write-Host "Directory completely removed."
}
else
{
    Write-Host "$FolderPath was not found."
}
