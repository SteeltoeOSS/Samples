Param(
	[string]$ShareName = "steeltoe_network_share",
	[string]$FolderPath = "c:\steeltoe_network_share",
	[string]$UserName = "shareWriteUser",
	[string]$Password = "thisIs1Pass!"
)
$ErrorActionPreference = "Stop"
#Requires -RunAsAdministrator
#Requires -Modules Microsoft.PowerShell.LocalAccounts, SmbShare
Import-Module Microsoft.PowerShell.LocalAccounts -SkipEditionCheck

$SecurePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force

if (Get-LocalUser -Name $UserName -ErrorAction SilentlyContinue)
{
    Write-Host "User $UserName already exists."
}
else
{
    Write-Host "Creating local user $UserName..."
    New-LocalUser $UserName `
        -Password $SecurePassword `
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

if (Get-Item -Path $FolderPath -ErrorAction SilentlyContinue)
{
    Write-Host "Directory $FolderPath already exists."
}
else
{
    Write-Host "Creating directory $FolderPath..."
    New-Item -ItemType directory -Path $FolderPath | Out-Null
    Write-Host "Done creating directory."
}

if (Get-SmbShare $ShareName -ErrorAction SilentlyContinue)
{
    Write-Host "SMB share $ShareName already exists."
}
else
{
    # Share the folder:
    # - allow all "Users" to read
    # - grant full control to current user and $UserName
    Write-Host "Creating SMB share '$ShareName'..."
    New-SmbShare -Name $ShareName `
    -Path $FolderPath `
    -ReadAccess "Everyone" `
    -FullAccess $UserName, $env:UserName | Out-Null
    Write-Host "Done creating share, now available at this path: \\$Env:COMPUTERNAME\$ShareName"
}
