## ECommerceApi

ASP.NET Core Web API for a simple e‑commerce backend, built with a layered architecture (Api / Application / Domain / Infrastrcutre).

### Features

- **Authentication & authorization**: ASP.NET Core Identity + JWT, role support (e.g. `Admin`).
- **Products**: public product catalog, admin CRUD.
- **Cart & wishlist**: per‑user cart and wishlist management.
- **Orders**: order placement, status updates, expiration background service.
- **Inventory**: stock tracking and adjustment.
- **Shipping addresses**: per‑user saved shipping addresses:
  - `GET /Api/Shipping/addresses` – list current user's addresses
  - `POST /Api/Shipping/addresses` – add new address
- **Payments**: Stripe integration and webhooks.

### Tech stack

- **.NET**: .NET 9 Web API
- **Database**: EF Core with SQLite (`app.db` – ignored from git)
- **Auth**: ASP.NET Core Identity + JWT
- **Caching / infra**: Redis, MongoDB (for products), Stripe

### Getting started

1. **Prerequisites**
   - .NET 9 SDK installed
   - SQLite
   - Optional but recommended: local Redis, MongoDB, and a Stripe account.

2. **Configuration**
   - Copy `appsettings.json` to `appsettings.Development.json` (if needed) and update:
     - `ConnectionStrings:DefaultConnection` – SQLite connection string
     - `JWT` settings – issuer, audience, signing key
     - `MongoDb`, `Redis`, and `Stripe` sections as appropriate
   - `appsettings.Development.json` is in `.gitignore` so secrets are not pushed.

3. **Database**
   - Apply migrations and create/update the SQLite database:

```bash
dotnet ef database update
```

4. **Run the API**

```bash
dotnet run
```

The API will start on the configured URL (by default `https://localhost:5001` / `http://localhost:5000`). Swagger UI is enabled in development.

### Notes for GitHub

- **Not committed**:
  - Build output: `bin/`, `obj/`
  - Local config with secrets: `appsettings.Development.json`
  - Local SQLite files: `*.db`, `*.db-shm`, `*.db-wal`
- If `app.db` and its `*-shm` / `*-wal` files are already tracked in your local git history, run (once) after this change to stop tracking them:

```bash
git rm --cached app.db app.db-shm app.db-wal
```

