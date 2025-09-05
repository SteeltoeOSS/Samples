#/usr/bin/env pwsh

<#
.SYNOPSIS
    Configure Sample projects build properties (Directory.Build.props).

.DESCRIPTION
    Sync Directory.Build.props in sample directories from templates at config/Directory.Build*.props.
    Projects should reference any Steeltoe package with Version=$(SteeltoeVersion).
#>

$repoRoot = "$PSScriptRoot"
$global:seenProjectFiles = @()
$global:seenPropsFiles = @()

function syncNet60() {
    $sourceFilePath = Resolve-Path -Path "$repoRoot/config/Directory.Build.net60.props"
    foreach ($projectFilePath in Get-ChildItem -Path $repoRoot -Recurse -Filter '*.csproj' | where { $_ | Select-String -SimpleMatch '<TargetFramework>net6.0</TargetFramework>' }) {
        $targetDir = Split-Path -Parent $projectFilePath
        $targetFilePath = Join-Path -Path $targetDir -ChildPath 'Directory.Build.props'
        Copy-Item $sourceFilePath -Destination $targetFilePath -Force

        $global:seenProjectFiles += $projectFilePath.FullName
        $global:seenPropsFiles += $targetFilePath
    }
}

function syncNet80() {
    $sourceFilePath = Resolve-Path -Path "$repoRoot/config/Directory.Build.net80.props"
    foreach ($projectFilePath in Get-ChildItem -Path $repoRoot -Recurse -Filter '*.csproj' | where { $_ | Select-String -SimpleMatch '<TargetFramework>net8.0</TargetFramework>' }) {
        $targetDir = Split-Path -Parent $projectFilePath
        $targetFilePath = Join-Path -Path $targetDir -ChildPath 'Directory.Build.props'
        Copy-Item $sourceFilePath -Destination $targetFilePath -Force

        $global:seenProjectFiles += $projectFilePath.FullName
        $global:seenPropsFiles += $targetFilePath
    }
}

function verify() {
    foreach ($projectFilePath in Get-ChildItem -Path $repoRoot -Recurse -Filter '*.csproj') {
        if ($global:seenProjectFiles -notcontains $projectFilePath.FullName) {
            Write-Warning "No Directory.Build.props synced for project at $projectFilePath"
        }
    }
    foreach ($projectFilePath in Get-ChildItem -Path $repoRoot -Recurse -Filter 'Directory.Build.props') {
        if ($global:seenPropsFiles -notcontains $projectFilePath.FullName) {
            Write-Warning "Directory.Build.props not synced at $projectFilePath"
        }
    }
}

syncNet60
syncNet80
verify

