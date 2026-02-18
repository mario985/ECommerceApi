# ECommerceApi

A modern **ASP.NET Core Web API** for a simple e-commerce backend, designed with clean architecture and real-world backend practices in mind.

This project focuses on **authentication, order lifecycle management, inventory control, caching, and payment integration**, making it suitable both as a learning project and a backend portfolio piece.

---

## 🧱 Architecture

The solution follows a **layered architecture**:

This separation keeps the system **maintainable, testable, and extensible**.

---

## ✨ Features

### 🔐 Authentication & Authorization
- ASP.NET Core Identity
- JWT access tokens
- Refresh tokens
- Role-based authorization (`Admin`, `User`)
- Token validation via middleware

---

### 🛒 Products
- Public product catalog
- Admin CRUD operations
- Products stored in **MongoDB (NoSQL)**

---

### ❤️ Cart & Wishlist
- Per-user cart management
- Wishlist support
- Inventory checks before checkout

---

### 📦 Orders
- Order creation and lifecycle management
- Order expiration via **Background Service**
- Status transitions (Pending → Paid → Completed / Expired)

---

### 📊 Inventory
- Stock tracking
- Reserved vs available quantities
- Automatic release of reserved stock for expired orders

---

### 🚚 Shipping Addresses
Per-user address management:
- `GET /api/shipping/addresses`
- `POST /api/shipping/addresses`

---

### 💳 Payments
- Stripe integration
- Secure webhook handling
- Signature verification

---

## ⚙️ Cross-Cutting Concerns

### 🧩 Middleware
- Global exception handling
- Request/response logging
- Authentication & authorization pipeline

### 🧪 Filters
- Centralized API response wrapping
- Validation handling
- Consistent error responses

### ⚡ Caching
- Redis caching layer
- Reduces database load
- Improves performance for frequently accessed data

### 🧠 MediatR
- CQRS-style request/handler pattern
- Decouples controllers from business logic
- Improves maintainability and testability

---

## 🛠 Tech Stack

- **.NET**: ASP.NET Core Web API (.NET 9)
- **Authentication**: ASP.NET Core Identity + JWT + Refresh Tokens
- **Databases**:
  - SQLite (EF Core) – relational data
  - MongoDB – product catalog (NoSQL)
- **Caching**: Redis
- **Payments**: Stripe + Webhooks
- **Patterns**: Layered Architecture, CQRS (MediatR)

---

## 🚀 Getting Started

### 1️⃣ Prerequisites
- .NET 9 SDK
- SQLite
- (Optional but recommended)
  - Redis
  - MongoDB
  - Stripe account (test mode)

---

### 2️⃣ Configuration

Copy and configure local settings:

```bash
cp appsettings.json appsettings.Development.json
Update:
ConnectionStrings:DefaultConnection
JWT (Issuer, Audience, Signing Key)
MongoDb, Redis
Stripe keys via environment variables
appsettings.Development.json is ignored from git to prevent leaking secrets.
3️⃣ Database
Apply migrations:
dotnet ef database update
4️⃣ Run the API
dotnet run
Swagger UI is enabled in development.
🔒 Security Notes
The following are intentionally not committed:
Build output: bin/, obj/
Local configuration with secrets
SQLite database files:
*.db
*.db-shm
*.db-wal
If database files were previously tracked, remove them once:
git rm --cached *.db *.db-shm *.db-wal
📌 Notes
Secrets are handled using environment variables
Stripe keys should be rotated if accidentally exposed
The project is structured for easy future extensions
📚 Purpose
This project was built to:
Practice real-world backend concepts
Apply clean architecture principles
Learn secure payment integration
Serve as a backend portfolio project