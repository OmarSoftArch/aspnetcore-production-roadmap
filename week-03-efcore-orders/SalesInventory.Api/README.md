# Week 03 - EF Core + Orders

This project completes the week 3 target:

- PostgreSQL through EF Core
- DbContext
- Migrations
- Seed data
- CRUD endpoints for Categories, Products, Customers, and Orders
- OrderItems with stock validation

## Connection string

Update `appsettings.json` if your local PostgreSQL password is not `postgres`:

```json
"ConnectionStrings": {
  "SalesInventoryDb": "Host=localhost;Port=5432;Database=sales_inventory_week3;Username=postgres;Password=postgres"
}
```

## Database setup

From the repository root:

```powershell
$env:USERPROFILE\.dotnet\tools\dotnet-ef.exe database update --project .\week-03-efcore-orders\SalesInventory.Api\SalesInventory.Api.csproj --startup-project .\week-03-efcore-orders\SalesInventory.Api\SalesInventory.Api.csproj
```

## Run

```powershell
dotnet run --project .\week-03-efcore-orders\SalesInventory.Api\SalesInventory.Api.csproj
```

Swagger opens in Development mode.
