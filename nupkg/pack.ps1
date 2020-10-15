# 执行公用脚本
. ".\common.ps1"

# 解决方案路径
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# 创建文件夹
if(!(Test-Path $packOutputFolder)){
    mkdir $packOutputFolder
}

# 解决方案还原依赖
Set-Location $slnPath
& dotnet restore

# 创建并移动过所有的 nuget 包到输出目录
foreach($project in $projects) {
    
    # 拼接项目目录
    $projectFolder = Join-Path $srcPath $project

    # 创建 nuget 包
    Set-Location $projectFolder
    Get-ChildItem (Join-Path $projectFolder "bin/Release") -ErrorAction SilentlyContinue | Remove-Item -Recurse
    & dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
    & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true

    # 复制 nuget 包
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packOutputFolder

}

# 返回脚本启动目录
Set-Location $packFolder