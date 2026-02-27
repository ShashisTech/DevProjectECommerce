# DevProjectECommerce — Architecture Document

Date: 2026-02-27

## Purpose
This document describes the high-level architecture, components, API interactions, data ownership, deployment, security, and operational guidance for the DevProjectECommerce microservices system (includes UI, UserService, CatalogService, OrderService, TransactionService, Auction/Bidding services).

## Diagram (Mermaid)
```mermaid
flowchart LR
  UI[UI - Angular SPA (ui-angular)]
  NG[Nginx / Reverse Proxy]

  subgraph Services
    US[UserService]
    CS[CatalogService]
    OS[OrderService]
    TS[TransactionService]
    AS[AuctionService]
    BS[BiddingService]
  end

  subgraph Datastores
    UDB[(User DB \n SQLite)]
    CDB[(Catalog DB \n SQLite)]
    ODB[(Order DB \n SQLite)]
    TDB[(Transaction DB \n SQLite)]
  end

  UI -->|HTTPS (JWT)| NG
  NG -->|HTTP| US
  NG -->|HTTP| OS
  NG -->|HTTP| CS
  NG -->|HTTP| AS
  NG -->|HTTP| BS

  OS -->|HTTP: GET /api/products/{id}\nPOST /api/products/{id}/reserve| CS
  OS -->|HTTP: POST /api/transactions (create payment)| TS
  TS -->|HTTP: GET /api/orders/{id} (validate)| OS

  US -->|owns auth| UDB
  CS -->|owns catalog data| CDB
  OS -->|owns orders| ODB
  TS -->|owns transactions| TDB

  %% Optional async/event-driven dashed connections
  OS -.->|events: OrderCreated| EventBus[(Optional Event Bus)]
  CS -.->|events: StockChanged| EventBus

  %% Deployment
  Docker[Docker Compose]
  NG --> Docker
  Services --> Docker
  Datastores --> Docker

  style EventBus stroke-dasharray: 5 5
  classDef optional fill:#fff4e6,stroke:#ff9900
  class EventBus optional
```

## Components & Responsibilities
- UI (ui-angular)
  - Single Page Application (Angular). Auth via JWT. Calls backend APIs through `nginx`.

- Nginx (Edge)
  - TLS termination, routing, and simple load balancing to service instances.

- UserService
  - Authentication and authorization (JWT). User registration, profile, roles, seller onboarding.
  - Database: `User DB` (service-owned). Seeds admin/buyer/seller on first run.

- CatalogService
  - Product and category CRUD, queries by category/name, and stock management.
  - Exposes `POST /api/products/{id}/reserve` to reserve/decrement product stock synchronously.
  - Database: `Catalog DB` (service-owned).

- OrderService
  - Receives order requests from UI.
  - Synchronous flow: call CatalogService to reserve stock; if successful, create local order record.
  - Database: `Order DB` (service-owned).

- TransactionService
  - Processes payments for orders. Validates order details with OrderService and records transaction.
  - Database: `Transaction DB` (service-owned).

- AuctionService / BiddingService
  - Auction/bid features (separate microservices); not modified here but part of solution.

## Key API Flows (synchronous)
1. Place order (Buy now)
   - UI -> OrderService POST /api/orders
   - OrderService -> CatalogService POST /api/products/{id}/reserve (quantity)
   - If reserved: OrderService writes Order and returns 201; then UI may call TransactionService to pay.
   - TransactionService -> OrderService GET /api/orders/{id} to validate and records Transaction.

2. Payment
   - UI/OrderService -> TransactionService POST /api/transactions with orderId and amount
   - TransactionService validates order and records payment; on failure, a compensation flow should release reserved stock.

## Data Ownership & Migrations
- Each service owns its DB and EF Core model. Use migrations per service.
- Services call `db.Database.Migrate()` at startup (Program.cs) for convenience; preferred approach: generate migrations and apply during CI/deploy.

Migrations commands (example run from repo root):
```powershell
# install dotnet-ef if needed
dotnet tool install --global dotnet-ef
# create migration for a service (example UserService)
dotnet ef migrations add InitialUserSchema -p src/UserService -s src/UserService
# apply migration
dotnet ef database update -p src/UserService -s src/UserService
```

## Security
- `UserService` issues JWTs. Other services validate JWTs for protected endpoints.
- Secrets (JWT signing keys, DB connection strings) must be stored securely (environment variables, secrets manager).
- Passwords hashed (sha256 in current scaffold) — replace with a stronger password hashing (e.g., Argon2/BCrypt) for production.

## Reliability & Concurrency
- Stock updates should be protected against race conditions:
  - Use DB-level transactions and optimistic concurrency (row version) in `CatalogService`.
  - Alternatively introduce a centralized inventory service or use an event-driven reservation with a durable store.
- Compensations/Saga:
  - Implement a saga (or choreography) to release reserved stock if payment fails.
  - Option A: Orchestrator service that coordinates reserve -> payment -> confirm/release.
  - Option B: Event-driven pattern: services emit `OrderCreated`, `PaymentSucceeded`, `PaymentFailed` events; `CatalogService` listens to release stock if payment fails.

## Deployment & Scaling
- Docker Compose exists for local dev. For production, run services on Kubernetes or a container platform; use a managed DB per service.
- Scale read-heavy services (CatalogService) horizontally; ensure DB can handle connections.

## Observability
- Add structured logging, distributed tracing (OpenTelemetry), metrics (Prometheus) and health probes for each service.

## Operational Checklist
- Add automated integration tests for the order → reserve → payment flow.
- Add migrations to source control and CI pipeline to apply them during deployment.
- Replace simple password hashing with PBKDF2/BCrypt/Argon2.
- Implement rate limiting and brute-force protection on auth endpoints.

## File references
- OrdersController: src/OrderService/Controllers/OrdersController.cs
- ProductsController (reserve): src/CatalogService/Controllers/ProductsController.cs
- TransactionService seed + client: src/TransactionService/Program.cs
- UserService auth and seeding: src/UserService/Program.cs and src/UserService/Controllers/AuthController.cs

## Next steps (recommended)
- Generate and commit EF migrations for `UserService`, `CatalogService`, `OrderService`, and `TransactionService`.
- Add automated tests and a CI job to build and run those tests.
- Implement a release/compensation flow for failed payments (event bus or orchestrator).

---

Generated and saved to repository root as `ARCHITECTURE.md` for printing.
