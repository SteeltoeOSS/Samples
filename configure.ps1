#/usr/bin/env pwsh

<#
.SYNOPSIS
    Configure Sample projects build properties (Directory.Build.props).

.DESCRIPTION
    Sync Directory.Build.props in project folder with "main" config/Directory.Build.props file.
    Project should reference any Steeltoe package with Version=$(SteeltoeVersion).
#>

$currentDirectory = Get-Location

# props file path to copy into sample projects
$propsFilePath = "./config/Directory.Build.props"

# copy props file only into samples that refer to Steeltoe libraries
$libraryReference = '$(SteeltoeVersion)'

function multitarget() {
    Write-Host 'multitarget projects...'
    try
    {
        Set-Location $PSScriptRoot

        $multitargets = '<TargetFrameworks>';
        $dirsToSync = Get-ChildItem *.csproj -Recurse | 
            where { $_ | Select-String -SimpleMatch $multitargets } |
            Select-String -SimpleMatch $libraryReference -List | Select Path | Split-Path

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
}

function net6only() {
    Write-Host 'net6.0 only projects...'
    try
    {
        Set-Location $PSScriptRoot

        $target6_0_only = '<TargetFramework>net6.0</TargetFramework>';
        $dirsToSync = Get-ChildItem *.csproj -Recurse |
            where { $_ | Select-String -SimpleMatch $target6_0_only } |
            Select-String -SimpleMatch $libraryReference -List | Select Path | Split-Path

        $propsAsXml = [xml](Get-Content $propsFilePath)

        $frameworkCondition = '''$(TargetFramework)'' == ''net6.0'''

        $propsAsXml.SelectNodes("//Project/PropertyGroup[@Condition != """ + $frameworkCondition + """]") |
            Foreach-Object {
                $_.ParentNode.RemoveChild($_)
            }

        #$propsAsXml.SelectSingleNode("//Project/PropertyGroup[@Condition = """ + $frameworkCondition + """]").Attributes.RemoveNamedItem("Condition");

        $propsFileName = Split-Path $propsFilePath -leaf

        foreach ($dir in $dirsToSync)
        {
            Write-Host $dir

            $pathToProjectProps = Join-Path -Path $dir -ChildPath $propsFileName
            $propsAsXml.Save($pathToProjectProps)
        }
    }
    finally
    {
        Set-Location $currentDirectory
    }
}

function net8only() {
    Write-Host 'net8.0 only projects...'
    try
    {
        Set-Location $PSScriptRoot

        $target8_0_only = '<TargetFramework>net8.0</TargetFramework>';
        $dirsToSync = Get-ChildItem *.csproj -Recurse |
            where { $_ | Select-String -SimpleMatch $target8_0_only } |
            Select-String -SimpleMatch $libraryReference -List | Select Path | Split-Path

        $propsAsXml = [xml](Get-Content $propsFilePath)

        $frameworkCondition = '''$(TargetFramework)'' == ''net8.0'''

        $propsAsXml.SelectNodes("//Project/PropertyGroup[@Condition != """ + $frameworkCondition + """]") |
            Foreach-Object {
                $_.ParentNode.RemoveChild($_)
            }

        #$propsAsXml.SelectSingleNode("//Project/PropertyGroup[@Condition = """ + $frameworkCondition + """]").Attributes.RemoveNamedItem("Condition");

        $propsFileName = Split-Path $propsFilePath -leaf

        foreach ($dir in $dirsToSync)
        {
            Write-Host $dir

            $pathToProjectProps = Join-Path -Path $dir -ChildPath $propsFileName
            $propsAsXml.Save($pathToProjectProps)
        }
    }
    finally
    {
        Set-Location $currentDirectory
    }
}


multitarget
net6only
net8only
