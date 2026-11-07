param(
    [string]$Version = 'v1.4.2'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    if (-not $Version) { $Version = (git describe --tags --abbrev=0) }

    # Build the self-contained single-file exe. $(SolutionDir) must be provided explicitly,
    # because the csproj uses it to locate gotri.exe (version/assembly info generation).
    dotnet publish 'NohBoard\NohBoard\NohBoard.csproj' -c Release -r win-x64 --self-contained true `
        -p:SolutionDir="$root\NohBoard\"
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE" }

    $publish = Join-Path $root 'NohBoard\NohBoard\bin\Release\netcoreapp3.1\win-x64\publish'
    $dist = Join-Path $root "dist\NohBoard-$Version-win-x64"
    if (Test-Path $dist) { Remove-Item $dist -Recurse -Force }
    New-Item -ItemType Directory -Path $dist | Out-Null

    # Single-file exe, the bundled default settings (transparent Input Overlay layout)
    # and the keyboards/styles that ship with the release.
    Copy-Item (Join-Path $publish 'NohBoard.exe') $dist
    Copy-Item (Join-Path $root 'packaging\NohBoard.json') $dist
    Copy-Item (Join-Path $root 'keyboards') $dist -Recurse
    Copy-Item (Join-Path $root 'README.md'), (Join-Path $root 'LICENSE') $dist
    Copy-Item (Join-Path $root 'docs') $dist -Recurse

    $zip = Join-Path $root "dist\NohBoard-$Version-win-x64.zip"
    if (Test-Path $zip) { Remove-Item $zip -Force }
    Compress-Archive -Path (Join-Path $dist '*') -DestinationPath $zip -CompressionLevel Optimal

    $exe = Get-Item (Join-Path $dist 'NohBoard.exe')
    $vi = $exe.VersionInfo
    Write-Host ("EXE : {0} ({1:N0} bytes) FileVersion={2} ProductVersion={3}" -f $exe.FullName, $exe.Length, $vi.FileVersion, $vi.ProductVersion)
    Write-Host ("DIST: {0} ({1:N2} MB)" -f $dist, ((Get-ChildItem $dist -Recurse -File | Measure-Object Length -Sum).Sum / 1MB))
    Write-Host ("ZIP : {0} ({1:N2} MB)" -f $zip, ((Get-Item $zip).Length / 1MB))
}
finally {
    Pop-Location
}
