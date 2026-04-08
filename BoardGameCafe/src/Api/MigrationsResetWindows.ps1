# reset-migrations.ps1
# DEV-ONLY SCRIPT — deletes ALL EF Core migrations
# Database does NOT need to be running

$ErrorActionPreference = "Stop"

Write-Host "Deleting ALL EF Core migrations (DEV ONLY)"
Write-Host "==============================================="

# Delete entire Migrations folder

$MigrationsPath = Join-Path $PSScriptRoot "Migrations"

if (Test-Path $MigrationsPath) {
    Remove-Item $MigrationsPath -Recurse -Force
    Write-Host "Migrations folder deleted"
} else {
    Write-Host "No Migrations folder found at $MigrationsPath"
}


if (Test-Path $MigrationsPath) {
    Remove-Item $MigrationsPath -Recurse -Force
    Write-Host "Migrations folder deleted"
} else {
    Write-Host "No Migrations folder found"
}

# Recreate migrations per DbContext
dotnet ef migrations add InitialCreate -c AccountManagementDbContext -o Migrations\AccountManagement

dotnet ef migrations add InitialCreate -c CafeFloorManagementDbContext -o Migrations\CafeFloorManagement

dotnet ef migrations add InitialCreate -c ContentAndPromotionsDbContext -o Migrations\ContentAndPromotions

dotnet ef migrations add InitialCreate -c EconomyManagementDbContext -o Migrations\EconomyManagement

dotnet ef migrations add InitialCreate -c GameCatalogManagementDbContext -o Migrations\GameCatalogManagement

dotnet ef migrations add InitialCreate -c NotificationsDbContext -o Migrations\Notifications

dotnet ef migrations add InitialCreate -c ReportDbContext -o Migrations\Report

dotnet ef migrations add InitialCreate -c ReservationManagementDbContext -o Migrations\ReservationManagement

Write-Host "InitialCreate migrations recreated"
