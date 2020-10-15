. ".\common.ps1"

$apiKey = $args[0]
if ([System.String]::IsNullOrWhiteSpace($apiKey)) 
{
    $apiKey = $env:NUGET_KEY
}

# 获取版本
[xml]$versionPropsXml = Get-Content (Join-Path $rootFolder "version.props")
$version = $versionPropsXml.Project.PropertyGroup.Version
$versionStr = $version.Trim()

# 发布所有包
foreach($project in $projects) {
    $projectName = $project
    $packageFullPath = Join-Path $packOutputFolder ($projectName + "." + $versionStr + ".nupkg")

    $packageFullPath

    & dotnet nuget push $packageFullPath -s https://api.nuget.org/v3/index.json --api-key "$apiKey" --skip-duplicate
}

# 返回脚本执行目录
Set-Location $packFolder
