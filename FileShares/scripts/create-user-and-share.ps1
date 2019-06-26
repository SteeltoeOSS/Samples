Param(
	[string]$shareName = "steeltoe_network_share",
	[string]$folderPath = "c:\steeltoe_network_share",
	[string]$username = "shareWriteUser",
	[string]$password = "thisIs1Pass!"
)
$ErrorActionPreference = "Stop"

$securePassword = ConvertTo-SecureString -String $password -AsPlainText -Force

# Create a local user account
New-LocalUser $username `
-Password $securePassword `
-FullName "NetworkFull Write" `
-Description "For write access to steeltoe_network_share"

Write-Host "-----> Created user accounts"

# Add accounts to users group
Add-LocalGroupMember -Group "Users" -Member $username
Write-Host "-----> Added to group"

# Create folders to share
New-Item -ItemType directory -Path $folderPath 
Write-Host "-----> Created folder"

# Share the folders, Users group gets read access
New-SmbShare -Name $shareName `
  -Path $folderPath `
  -ReadAccess "Everyone" `
  -FullAccess $username, $env:UserName

Write-Host "-----> Share address: \\$Env:COMPUTERNAME\$shareName"