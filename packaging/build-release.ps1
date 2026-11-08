param(
    [string]$Version = 'v1.5.0',
    # Only build the lite package. Useful while iterating on the launcher, because the self-contained publish
    # produces a ~65 MB exe and takes a while. The default (without the switch) builds both packages.
    [switch]$LiteOnly
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    if (-not $Version) { $Version = (git describe --tags --abbrev=0) }

    $distRoot = Join-Path $root 'dist'
    $releaseRoot = Join-Path $root 'NohBoard\NohBoard\bin\Release\net8.0-windows\win-x64'
    $publishSelfContained = Join-Path $releaseRoot 'publish'
    $publishFrameworkDependent = Join-Path $root 'artifacts\publish-lite'

    # Lite launcher: a tiny .NET Framework executable that checks for the .NET 8 Desktop Runtime, shows a
    # download hint when it is missing and otherwise starts the framework-dependent NohBoard.Core.exe.
    $launcherSource = Join-Path $root 'packaging\lite-launcher\Launcher.cs'
    $launcherIcon = Join-Path $root 'NohBoard\NohBoard\NohBoard2.ico'
    $launcherExeName = 'NohBoard.exe'
    $coreExeName = 'NohBoard.Core.exe'

    function Get-CSharpCompiler {
        $windowsDirectory = if ($env:WINDIR) { $env:WINDIR } else { $env:SystemRoot }
        $candidates = @(
            (Join-Path $windowsDirectory 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'),
            (Join-Path $windowsDirectory 'Microsoft.NET\Framework\v4.0.30319\csc.exe')
        )

        foreach ($candidate in $candidates) {
            if (Test-Path $candidate) { return $candidate }
        }

        $message = '找不到 .NET Framework C# 编译器 csc.exe（已尝试：{0}）。' -f ($candidates -join '; ')
        $message += 'lite 启动器依赖 Windows 自带的 .NET Framework 4.x 编译器，请确认系统安装完整。'
        throw $message
    }

    function New-LiteLauncher([string]$outputPath) {
        if (-not (Test-Path $launcherSource)) { throw "lite 启动器源码不存在：$launcherSource" }
        if (-not (Test-Path $launcherIcon)) { throw "lite 启动器图标不存在：$launcherIcon" }

        $csc = Get-CSharpCompiler
        $cscArguments = @(
            '/nologo',
            '/target:winexe',
            '/platform:anycpu',
            '/optimize+',
            ('/win32icon:{0}' -f $launcherIcon),
            ('/out:{0}' -f $outputPath),
            $launcherSource
        )

        Write-Host ('Compiling lite launcher with {0}' -f $csc)
        & $csc @cscArguments
        if ($LASTEXITCODE -ne 0) { throw "csc.exe 编译 lite 启动器失败，退出码 $LASTEXITCODE" }
        if (-not (Test-Path $outputPath)) { throw "csc.exe 未生成 lite 启动器：$outputPath" }
    }

    function New-Package([string]$name, [string[]]$executablePaths, [string]$note) {
        $dist = Join-Path $distRoot $name
        if (Test-Path $dist) { Remove-Item $dist -Recurse -Force }
        New-Item -ItemType Directory -Path $dist | Out-Null

        # Copy one or more executables: the standalone package ships a single NohBoard.exe, the lite package
        # ships the launcher NohBoard.exe plus the framework-dependent NohBoard.Core.exe.
        $details = @()
        foreach ($executablePath in $executablePaths) {
            if (-not (Test-Path $executablePath)) { throw "要打包的文件不存在：$executablePath" }
            Copy-Item $executablePath $dist
            $exe = Get-Item (Join-Path $dist (Split-Path -Leaf $executablePath))
            $fileVersion = $exe.VersionInfo.FileVersion
            if (-not $fileVersion -or $fileVersion -eq '0.0.0.0') { $fileVersion = 'n/a' }
            $details += ('{0} {1:N2} MB (FileVersion {2})' -f $exe.Name, ($exe.Length / 1MB), $fileVersion)
        }

        Copy-Item (Join-Path $root 'packaging\NohBoard.json') $dist
        Copy-Item (Join-Path $root 'keyboards') $dist -Recurse
        Copy-Item (Join-Path $root 'README.md'), (Join-Path $root 'LICENSE') $dist
        Copy-Item (Join-Path $root 'docs') $dist -Recurse
        if ($note) { Set-Content -Path (Join-Path $dist 'README-FIRST.txt') -Value $note -Encoding UTF8 }

        $zip = Join-Path $distRoot "$name.zip"
        if (Test-Path $zip) { Remove-Item $zip -Force }
        Compress-Archive -Path (Join-Path $dist '*') -DestinationPath $zip -CompressionLevel Optimal

        $zipSize = (Get-Item $zip).Length / 1MB
        Write-Host ('{0}: {1}; zip {2:N2} MB' -f $name, ($details -join ' + '), $zipSize)
        return $zip
    }

    if ($LiteOnly) {
        Write-Host 'LiteOnly: 跳过自包含（独立版）发布，只构建 lite 包。'
    }
    else {
        # Standalone build: one self-contained single-file exe with the whole .NET runtime, compressed
        # (EnableCompressionInSingleFile is set in the csproj). Works on machines without .NET installed.
        dotnet publish 'NohBoard\NohBoard\NohBoard.csproj' -c Release -r win-x64 --self-contained true `
            -p:SolutionDir="$root\NohBoard\"
        if ($LASTEXITCODE -ne 0) { throw "dotnet publish (self-contained) failed with exit code $LASTEXITCODE" }
    }

    # Lite build: a small single-file executable that needs the .NET 8 Desktop Runtime on the target machine.
    # Single-file compression is not supported for framework-dependent apps, so it is disabled here.
    if (Test-Path $publishFrameworkDependent) { Remove-Item $publishFrameworkDependent -Recurse -Force }
    dotnet publish 'NohBoard\NohBoard\NohBoard.csproj' -c Release -r win-x64 --self-contained false `
        -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=false `
        -p:SolutionDir="$root\NohBoard\" -o $publishFrameworkDependent
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish (framework-dependent) failed with exit code $LASTEXITCODE" }

    # Turn the framework-dependent apphost into NohBoard.Core.exe and put the launcher in its place as
    # NohBoard.exe. Users see one entry point; the launcher explains a missing runtime before .NET is involved.
    $frameworkDependentExe = Join-Path $publishFrameworkDependent $launcherExeName
    if (-not (Test-Path $frameworkDependentExe)) {
        throw "框架依赖发布未生成 $launcherExeName：$publishFrameworkDependent"
    }
    $coreExePath = Join-Path $publishFrameworkDependent $coreExeName
    if (Test-Path $coreExePath) { Remove-Item $coreExePath -Force }
    Rename-Item -Path $frameworkDependentExe -NewName $coreExeName
    New-LiteLauncher $frameworkDependentExe

    $liteNote = @"
NohBoard lite ($Version)

这个版本是“精简版”：exe 很小，但需要目标电脑安装 .NET 8 Desktop Runtime（Windows Desktop Runtime 8.x，x64）。
启动时如果检测不到 .NET 8 桌面运行时，NohBoard 会弹窗说明缺少什么，并可一键打开官方下载页面：
https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-desktop-8.0.33-windows-x64-installer
（也可以手动下载 Windows Desktop Runtime 8.x x64 安装包；安装完成后重新双击 NohBoard.exe 即可。）

不想装运行时的用户请下载 NohBoard-$Version-win-x64.zip（独立版，约 65 MB，无需安装 .NET）。
"@

    $litePackageName = "NohBoard-$Version-win-x64-lite"
    if ($LiteOnly) {
        $liteZip = New-Package $litePackageName @($frameworkDependentExe, $coreExePath) $liteNote
        Write-Host ''
        Write-Host "Lite      : $liteZip"
    }
    else {
        $standaloneZip = New-Package "NohBoard-$Version-win-x64" @((Join-Path $publishSelfContained 'NohBoard.exe')) $null
        $liteZip = New-Package $litePackageName @($frameworkDependentExe, $coreExePath) $liteNote
        Write-Host ''
        Write-Host "Standalone: $standaloneZip"
        Write-Host "Lite      : $liteZip"
    }

    # Make sure the lite package really contains both executables.
    $liteDist = Join-Path $distRoot $litePackageName
    foreach ($requiredName in @($launcherExeName, $coreExeName)) {
        if (-not (Test-Path (Join-Path $liteDist $requiredName))) {
            throw "lite 包中缺少 $requiredName：$liteDist"
        }
    }
    if (-not (Test-Path (Join-Path $liteDist 'keyboards'))) {
        throw "lite 包中缺少 keyboards 目录：$liteDist"
    }
}
finally {
    Pop-Location
}
