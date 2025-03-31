Param(
	[string]$ShareName = "steeltoe_network_share",
	[string]$FolderPath = "c:\steeltoe_network_share",
	[string]$UserName = "shareWriteUser",
	[string]$Password = "thisIs1Pass!"
)
$ErrorActionPreference = "Stop"

$SecurePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force

Write-Host "Creating local user $UserName..."
New-LocalUser $UserName `
    -Password $SecurePassword `
    -FullName "SMB ReadWrite" `
    -Description "For write access to $ShareName"
Write-Host "Done creating user."

Write-Host "Adding $UserName to 'Users' group..."
Add-LocalGroupMember -Group "Users" -Member $UserName
Write-Host "Done adding user to group."

Write-Host "Creating directory $FolderPath..."
New-Item -ItemType directory -Path $FolderPath
Write-Host "Done creating folder."

# Share the folder:
# - allow all "Users" to read
# - grant full control to current user and $UserName
Write-Host "Creating network share '$ShareName'..."
New-SmbShare -Name $ShareName `
  -Path $FolderPath `
  -ReadAccess "Everyone" `
  -FullAccess $UserName, $env:UserName
Write-Host "Done creating share, now available at this path: \\$Env:COMPUTERNAME\$ShareName"
