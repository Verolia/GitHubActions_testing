#!/bin/bash
set -e  # Stop on first error

echo "Deleting ALL EF Core migrations (DEV ONLY)"
echo "=============================================="

# Delete the entire Migrations folder
rm -rf Migrations

echo "Migrations folder deleted"

# Recreate migrations for each DbContext
dotnet ef migrations add InitialCreate -c AccountManagementDbContext -o Migrations\AccountManagement

dotnet ef migrations add InitialCreate -c CafeFloorManagementDbContext -o Migrations\CafeFloorManagement

dotnet ef migrations add InitialCreate -c ContentAndPromotionsDbContext -o Migrations\ContentAndPromotions

dotnet ef migrations add InitialCreate -c EconomyManagementDbContext -o Migrations\EconomyManagement

dotnet ef migrations add InitialCreate -c GameCatalogManagementDbContext -o Migrations\GameCatalogManagement

dotnet ef migrations add InitialCreate -c NotificationsDbContext -o Migrations\Notifications

dotnet ef migrations add InitialCreate -c ReportDbContext -o Migrations\Report

dotnet ef migrations add InitialCreate -c ReservationManagementDbContext -o Migrations\ReservationManagement

echo "InitialCreate migrations recreated"