#/usr/bin/env pwsh

<#
.SYNOPSIS
    Configure Sample projects build properties (Directory.Build.props).

.DESCRIPTION
    Sync Directory.Build.props in project folder with "main" config/Directory.Build.props file.
    Project should reference to any Steeltoe package with Version=$(SteeltoeVersion).
#>

$currentDirectory = Get-Location

# props file path to copy into sample projects
$propsFilePath = "./config/Directory.Build.props"

# copy props file only into samples which refers to Steeltoe libraries
$libraryReference = '$(SteeltoeVersion)'

try
{
    Set-Location $PSScriptRoot

    $dirsToSync = Get-ChildItem *.csproj -Recurse | Select-String -SimpleMatch $libraryReference -List | Select Path | Split-Path

    foreach ($dir in $dirsToSync)
    {
        Write-Host $dir
        Copy-Item $propsFilePath $dir -Force
    }
}
finally
{
    Set-Location $currentDirectory
}
