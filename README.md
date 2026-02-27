DevProjectECommerce

Solution with microservices (Catalog, Auction, Order, Transaction) and an Angular UI.

Run locally:
- Install .NET 8 SDK (or 7) and Node.js + Angular CLI
- For each service:
  - dotnet run --project src\CatalogService
  - dotnet run --project src\AuctionService
  - dotnet run --project src\OrderService
  - dotnet run --project src\TransactionService

Docker: Each service has a Dockerfile at src/<Service>/Dockerfile

Azure deployment:
- Create an Azure Container Registry
- Configure GitHub Actions secrets used in .github/workflows/ci-cd.yml
- Create four App Services for Containers and point each to the respective image in ACR
