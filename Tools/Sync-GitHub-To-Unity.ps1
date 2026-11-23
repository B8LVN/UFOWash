# UFO WASH: pull GitHub changes into the Unity project that is already open.
$ErrorActionPreference = "Stop"
$repo = "C:\UnityProjects\UFOWash-Repo"
$unity = "C:\UnityProjects\UFOWash"

if (!(Test-Path (Join-Path $repo ".git"))) { throw "Git clone not found: $repo" }
if (!(Test-Path (Join-Path $unity "Assets"))) { throw "Unity project not found: $unity" }

Write-Host "[1/4] Checking local clone..." -ForegroundColor Cyan
$status = git -C $repo status --porcelain
if ($LASTEXITCODE -ne 0) { throw "Git status failed." }
if ($status) {
    Write-Host "Your clone has local changes. Nothing was overwritten. Commit/stash them first, then rerun." -ForegroundColor Yellow
    exit 2
}

Write-Host "[2/4] Pulling latest UFO WASH files from GitHub..." -ForegroundColor Cyan
git -C $repo pull --ff-only origin main
if ($LASTEXITCODE -ne 0) { throw "Git pull failed. Resolve the Git issue before syncing." }

$stamp = Get-Date -Format "yyyyMMdd-HHmmss"
$backup = Join-Path (Split-Path $unity -Parent) "UFOWash-SyncBackup-$stamp"
New-Item -ItemType Directory -Path $backup -Force | Out-Null

Write-Host "[3/4] Backing up the Unity project folders..." -ForegroundColor Cyan
foreach ($folder in @("Assets","Packages","ProjectSettings")) {
    $source = Join-Path $unity $folder
    if (Test-Path $source) {
        robocopy $source (Join-Path $backup $folder) /E /R:1 /W:1 /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "Backup failed for $folder." }
    }
}

Write-Host "[4/4] Copying GitHub files into the actual Unity project..." -ForegroundColor Cyan
foreach ($folder in @("Assets","Packages","ProjectSettings")) {
    $source = Join-Path $repo $folder
    $destination = Join-Path $unity $folder
    if (Test-Path $source) {
        robocopy $source $destination /E /R:2 /W:1 /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "Copy failed for $folder." }
    }
}

Write-Host ""
Write-Host "SYNC COMPLETE. Unity project updated: $unity" -ForegroundColor Green
Write-Host "Backup saved at: $backup" -ForegroundColor Green
Write-Host "Return to Unity and wait for compilation/import to finish." -ForegroundColor Green
Write-Host "Then use menu: UFO WASH > BUILD PLAYABLE CLEANING PROTOTYPE" -ForegroundColor Green
