#Requires -RunAsAdministrator
#Requires -Modules Microsoft.PowerShell.LocalAccounts, SmbShare

Param(
    [Parameter(Mandatory = $false)][string]$ShareName = "steeltoe_network_share",
    [Parameter(Mandatory = $false)][string]$SharePath = "c:\steeltoe_network_share",
    [Parameter(Mandatory = $false)][string]$UserName = "shareWriteUser",
    [Parameter(Mandatory = $false)][string]$Password = "thisIs1Pass!"
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
$securePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force

if (Get-LocalUser -Name $UserName -ErrorAction SilentlyContinue)
{
    Write-Host "User $UserName already exists."
}
else
{
    Write-Host "Creating local user $UserName..."
    New-LocalUser $UserName `
        -Password $securePassword `
        -FullName "SMB ReadWrite" `
        -Description "For write access to $ShareName" | Out-Null
    Write-Host "Done creating user."
}

if (Get-LocalGroupMember -Group "Users" -Member $UserName -ErrorAction SilentlyContinue)
{
    Write-Host "$UserName is already a member of the 'Users' group."
}
else
{
    Write-Host "Adding $UserName to 'Users' group..."
    Add-LocalGroupMember -Group "Users" -Member $UserName
    Write-Host "Done adding user to group."
}

if (Get-Item -Path $SharePath -ErrorAction SilentlyContinue)
{
    Write-Host "Directory $SharePath already exists."
}
else
{
    Write-Host "Creating directory $SharePath..."
    New-Item -ItemType directory -Path $SharePath | Out-Null
    Write-Host "Done creating directory."
}

if (Get-SmbShare $ShareName -ErrorAction SilentlyContinue)
{
    Write-Host "SMB share $ShareName already exists."
}
else
{
    # Share the directory:
    # - allow all "Users" to read
    # - grant full control to current user and $UserName
    Write-Host "Creating SMB share '$ShareName'..."
    New-SmbShare -Name $ShareName `
        -Path $SharePath `
        -ReadAccess "Everyone" `
        -FullAccess $UserName, $env:UserName | Out-Null
    Write-Host "Done creating share, now available at this path: \\$Env:COMPUTERNAME\$ShareName"
}
