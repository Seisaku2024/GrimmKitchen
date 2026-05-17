# 1. 修正対象ファイル名（ファイル名のみを指定）
$targetFileNames = @(
    "ltsmulti_gem.shader",
    "lts_ref_blur.shader",
    "ltspass_tess_transparent.shader",
    "lts_gem.shader",
    "lts_fur_cutout.shader",
    "ltsmulti_o.shader",
    "ltsmulti.shader",
    "ltspass_lite_transparent.shader",
    "ltspass_tess_opaque.shader",
	"ltspass_tess_cutout.shader",
    "ltspass_opaque.shader",
    "ltspass_cutout.shader",
    "ltspass_lite_opaque.shader",
	"lts_fur.shader",
    "lts_fur_two.shader",
    "ltspass_lite_cutout.shader",
    "ltspass_transparent.shader",
	"ltsmulti_fur.shader",
    "ltsmulti_ref.shader"
)

# 2. パスの構築
# スクリプト実行位置を基準に lilToon の Shader フォルダを指定
$currentDir = Get-Location
$shaderDir = Join-Path $currentDir "Packages\jp.lilxyzw.liltoon\Shader"

# 3. 設定
$keywords = @(
    "_ADDITIONAL_LIGHT_SHADOWS",
    "_SCREEN_SPACE_OCCLUSION",
    "_DBUFFER_MRT1", "_DBUFFER_MRT2", "_DBUFFER_MRT3",
    "LIGHTMAP_SHADOW_MIXING",
    "SHADOWS_SHADOWMASK",
    "DIRLIGHTMAP_COMBINED",
    "LIGHTMAP_ON",
    "DYNAMICLIGHTMAP_ON",
    "_MAIN_LIGHT_SHADOWS",
    "_REFLECTION_PROBE_BLENDING",
    "_REFLECTION_PROBE_BOX_PROJECTION"
)

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$regexInner = ($keywords | ForEach-Object { [regex]::Escape($_) }) -join "|"
$keywordRegex = "#pragma\s+multi_compile.*\b($regexInner)\b"

Write-Host "Target Directory: $shaderDir" -ForegroundColor White -BackgroundColor DarkGreen

foreach ($fileName in $targetFileNames) {
    $fullPath = Join-Path $shaderDir $fileName
    
    if (-not (Test-Path $fullPath)) {
        Write-Host "[-] Skip: File not found -> $fileName" -ForegroundColor Gray
        continue
    }

    # 属性解除
    Set-ItemProperty -Path $fullPath -Name IsReadOnly -Value $false

    # UTF-8で読み込み
    $lines = [System.IO.File]::ReadAllLines($fullPath, $utf8NoBom)
    $newLines = New-Object System.Collections.Generic.List[string]
    $fileModified = $false

    foreach ($line in $lines) {
        # 二重コメントアウト防止
        if ($line -match "^\s*//") {
            $newLines.Add($line)
            continue
        }

        # キーワードが含まれる #pragma 行か判定
        if ($line -match $keywordRegex) {
            $index = $line.IndexOf("#")
            $newLines.Add($line.Insert($index, "//"))
            $fileModified = $true
        } else {
            $newLines.Add($line)
        }
    }

    if ($fileModified) {
        [System.IO.File]::WriteAllLines($fullPath, $newLines, $utf8NoBom)
        Write-Host "[!] Fixed: $fileName" -ForegroundColor Cyan
    } else {
        Write-Host "[?] No change needed: $fileName" -ForegroundColor Yellow
    }

    # 再上書き防止のロック
    Set-ItemProperty -Path $fullPath -Name IsReadOnly -Value $true
}

Write-Host "`nAll specified files in the directory have been processed." -ForegroundColor Green