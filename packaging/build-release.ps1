param(
    [string]$Version = 'v1.5.0'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    if (-not $Version) { $Version = (git describe --tags --abbrev=0) }

    $releaseRoot = Join-Path $root 'NohBoard\NohBoard\bin\Release\net8.0-windows\win-x64'
    $publishSelfContained = Join-Path $releaseRoot 'publish'
    $publishFrameworkDependent = Join-Path $root 'artifacts\publish-lite'

    # Standalone build: one self-contained single-file exe with the whole .NET runtime, compressed
    # (EnableCompressionInSingleFile is set in the csproj). Works on machines without .NET installed.
    dotnet publish 'NohBoard\NohBoard\NohBoard.csproj' -c Release -r win-x64 --self-contained true `
        -p:SolutionDir="$root\NohBoard\"
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish (self-contained) failed with exit code $LASTEXITCODE" }

    # Lite build: a small single-file exe that needs the .NET 8 Desktop Runtime on the target machine.
    # Single-file compression is not supported for framework-dependent apps, so it is disabled here.
    if (Test-Path $publishFrameworkDependent) { Remove-Item $publishFrameworkDependent -Recurse -Force }
    dotnet publish 'NohBoard\NohBoard\NohBoard.csproj' -c Release -r win-x64 --self-contained false `
        -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=false `
        -p:SolutionDir="$root\NohBoard\" -o $publishFrameworkDependent
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish (framework-dependent) failed with exit code $LASTEXITCODE" }

    function New-Package([string]$name, [string]$exePath, [string]$note) {
        $dist = Join-Path $root "dist\$name"
        if (Test-Path $dist) { Remove-Item $dist -Recurse -Force }
        New-Item -ItemType Directory -Path $dist | Out-Null

        Copy-Item $exePath $dist
        Copy-Item (Join-Path $root 'packaging\NohBoard.json') $dist
        Copy-Item (Join-Path $root 'keyboards') $dist -Recurse
        Copy-Item (Join-Path $root 'README.md'), (Join-Path $root 'LICENSE') $dist
        Copy-Item (Join-Path $root 'docs') $dist -Recurse
        if ($note) { Set-Content -Path (Join-Path $dist 'README-FIRST.txt') -Value $note -Encoding UTF8 }

        $zip = Join-Path $root "dist\$name.zip"
        if (Test-Path $zip) { Remove-Item $zip -Force }
        Compress-Archive -Path (Join-Path $dist '*') -DestinationPath $zip -CompressionLevel Optimal

        $exe = Get-Item (Join-Path $dist 'NohBoard.exe')
        $vi = $exe.VersionInfo
        Write-Host ("{0}: exe {1:N2} MB, FileVersion={2}, zip {3:N2} MB" -f `
            $name, ($exe.Length / 1MB), $vi.FileVersion, ((Get-Item $zip).Length / 1MB))
        return $zip
    }

    $standaloneZip = New-Package "NohBoard-$Version-win-x64" (Join-Path $publishSelfContained 'NohBoard.exe') $null
    $liteNote = @"
NohBoard lite ($Version)

这个版本是“精简版”：exe 很小，但需要目标电脑安装 .NET 8 Desktop Runtime（Windows Desktop Runtime 8.x）。
如果双击后提示缺少 .NET，请安装：
https://dotnet.microsoft.com/download/dotnet/8.0  （选择 Windows Desktop Runtime x64）

不想装运行时的用户请下载 NohBoard-$Version-win-x64.zip（独立版，约 65 MB，无需安装 .NET）。
"@
    $liteZip = New-Package "NohBoard-$Version-win-x64-lite" (Join-Path $publishFrameworkDependent 'NohBoard.exe') $liteNote

    Write-Host ""
    Write-Host "Standalone: $standaloneZip"
    Write-Host "Lite      : $liteZip"
}
finally {
    Pop-Location
}
