Param(
    [string]$ShareName = "steeltoe_network_share",
    [string]$FolderPath = "c:\steeltoe_network_share",
    [string]$UserName = "shareWriteUser"
)
$ErrorActionPreference = "Stop"

Remove-SmbShare -Name $ShareName
Write-Host "SMB share removed."

Write-Host "Removing $UserName from local 'Users' group..."
Remove-LocalGroupMember -Group "Users" -Member $UserName
Write-Host "User removed from group."

Write-Host "Removing local user $UserName..."
Remove-LocalUser -Name $UserName
Write-Host "User completely removed."

Write-Host "Removing $FolderPath from disk..."
Remove-Item -Path $FolderPath -Recurse
Write-Host "Folder completely removed."
