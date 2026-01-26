## ECommerceApi

This project is a learning/back‑end playground for building a small e‑commerce API using **ASP.NET Core 9**, **Entity Framework Core (SQLite)**, **ASP.NET Identity + JWT**, **MongoDB**, **Redis**, **MediatR**, and **AutoMapper**.

The notes below summarize what is already implemented and what is still missing or can be improved, so you can quickly get back into the code after a break.

---

## What is implemented

- **Solution & Infrastructure**
  - **Target framework**: `net9.0`.
  - **EF Core + SQLite** for relational data (`AppDbContext`, migrations in `Migrations`).
  - **ASP.NET Identity** integrated with `User` entity and seeding of roles/admin (`IdentitySeeder`, `RegisterRoles`).
  - **Global API response filter** and **exception handling middleware** wired in `Program.cs`.
  - **Swagger / OpenAPI** with JWT bearer security definition.
  - **MongoDB** configured via `MongoDbSetting` and `MongoDbInitializer` (used mainly for product catalog).
  - **Redis cache** configured using `StackExchange.Redis` and wrapped by `RedisCacheService`.
  - **MediatR** for domain events / commands (product created / updated, stock availability).

- **Domain Model**
  - **Users & Auth**
    - `User` entity with navigation to `RefreshToken`.
    - `RefreshToken` entity with expiration / revoked flags.
  - **Catalog**
    - `Product` entity with `IsAvailable`, category, price, etc.
    - `Inventory` entity to track product stock.
  - **Cart & Wishlist**
    - `Cart` / `CartItem` entities.
    - `WishList` / `WishListItem` entities.
  - **Orders**
    - `Order` / `OrderItem` entities.
    - `OrderStatus`, `PaymentMethod`, `ProductCategory` enums.

- **Repositories & Unit of Work**
  - Interfaces in `Domain/Interfaces` implemented in `Infrastrcutre/Repositories`:
    - `IProductRepository`, `ICartRepository`, `IWishListRepository`,
      `IInventoryRepository`, `IOrderRepository`, `IUserRepository`.
  - `UnitOfWork` with explicit transaction control used in `OrderService`.

- **Application Services & Business Logic**
  - **Authentication (`AuthService`)**
    - Register user via `IUserRepository.AddAsync`.
    - Login with email/password validation.
    - JWT generation via `TokenService`.
    - Refresh token generation, rotation, and persistence via `RefreshTokenService`.
    - Support for:
      - `RefreshTokenAsync`
      - `RevokeTokenAsync`
      - `RevokeAllTokensAsync`
  - **Product Catalog**
    - `ProductService`:
      - Get products with filtering + pagination using `ProdcutFilterDto`.
      - Get single product by id.
      - **Redis caching** for product lists and single product:
        - Cache key based on filter parameters.
        - Cache invalidation performed by `AdminProductService` on update/delete.
    - `AdminProductService`:
      - Create product (`CreateProductDto`) with `IsAvailable` computed from quantity.
      - Update product (`UpdateProductDto`) with:
        - Redis invalidation for product cache.
        - Publish `ProductUpdatedCommand` via MediatR.
      - Delete product with Redis invalidation.
      - Change availability and invalidate cache.
    - Command handlers (MediatR) implemented:
      - `ProductCreatedHandler`
      - `UpdateProductHandler`
      - `StockAvailabilityHandler`
  - **Cart**
    - `CartService`:
      - Add item to cart (`AddToCartDto`).
      - Remove item (`RemoveFromCartDto`).
      - Clear cart for user.
      - Update item quantity (`UpdateCartItemQuantityDto`, including "quantity 0 = remove item").
      - Get or create cart by user id.
      - Maps `Cart` → `CartDto` and enriches items with `ProductDto` using `IProductRepository.GetByIdsAsync`.
  - **Wishlist**
    - `WishListService`:
      - Get wishlist (creates empty wishlist if none exists).
      - Add item (ensures product exists and not duplicated).
      - Remove item and clear wishlist.
      - Maps `WishList` → `WishListDto` and enriches with `ProductDto` similar to cart.
  - **Orders**
    - `OrderService`:
      - `PlaceOrderAsync(userId)`:
        - Validates that cart exists and has items.
        - Starts transaction via `IUnitOfWork`.
        - Checks inventory for each cart item and reduces quantity.
        - Creates `Order` + `OrderItems` with total price and `Pending` status.
        - Clears cart.
      - `GetAsync(id)` and `GetByUserIdAsync(userId)` implemented.

- **API Endpoints (Controllers)**
  - `AccountController` (`Api/Account`):
    - `POST /Api/Account/Register` – creates user via `RegisterCommand`.
    - `POST /Api/Account/LogIn` – login, issues JWT + refresh token cookie.
    - `GET /Api/Account/token` – uses refresh token from cookie to issue new tokens.
    - `POST /Api/Account/revokeToken` – revoke a specific refresh token.
    - `POST /Api/Account/revokeAll` – revoke all active tokens for user.
    - `GET /Api/Account/test` – Admin-only test endpoint.
  - `ProductController` (`Api/Product`):
    - `GET /Api/Product` – list products with filtering (`ProdcutFilterDto`).
    - `GET /Api/Product/{id}` – get single product by id.
  - `AdminProductController` (`Api/AdminProduct`, Admin only):
    - `POST /Api/AdminProduct` – create product.
    - `PUT /Api/AdminProduct/{id}` – update product.
    - `DELETE /Api/AdminProduct/{id}` – delete product.
  - `CartController` (`Api/Cart`, authorized users):
    - `POST /Api/Cart/Add` – add item.
    - `DELETE /Api/Cart/remove/{userId}/{productId}` – remove specific item.
    - `POST /Api/Cart/Clear` – clear user cart.
    - `POST /Api/Cart/updateQuantity` – change quantity (or remove if zero).
    - `GET /Api/Cart/{userId}` – get cart for user.
  - `WishListController` (`Api/WishList`, authorized users):
    - `GET /Api/WishList` – get current user wishlist.
    - `POST /Api/WishList` – add item (user id is taken from JWT).
    - `DELETE /Api/WishList/{productId}` – remove product for current user.
    - `DELETE /Api/WishList/clear` – clear wishlist for current user.
  - `OrderController` (`Api/Order`, authorized users):
    - `POST /Api/Order` – place order for current user (uses cart + inventory).

---

## What is left / possible next steps

These are not strict requirements, but realistic next steps and missing pieces based on the current code:

- **Documentation & DX**
  - Add concrete request/response examples to this `README` for main endpoints.
  - Document expected JWT and refresh token flow more clearly.
  - Add high‑level architecture diagram or short notes about how SQL, MongoDB, Redis, and Identity work together.

- **Auth & Security**
  - Confirm password policies, email confirmation, and lock‑out behavior (currently not configured explicitly).
  - Add endpoints for updating profile data, changing password, and possibly role management (admin UI/API).
  - Enforce HTTPS and secure cookie options (`Secure`, `SameSite`) for the refresh token cookie in production.

- **Products & Catalog**
  - Expose endpoints for **listing products by category/brand**, featured products, etc., if needed by the client.
  - Add endpoints to view product inventory or availability for admins.
  - Consider cache invalidation for the **product list** keys (currently only single product cache is explicitly invalidated).

- **Cart & Wishlist**
  - Unify how user id is determined:
    - Some cart endpoints accept `userId` in route/body, while wishlist uses the authenticated user from JWT.
    - Next step: move cart to use authenticated user id consistently and drop explicit `userId` from routes.
  - Add validation/error responses for invalid product ids or inventory in cart operations.

- **Orders**
  - Add endpoints to:
    - Get order by id (`GET /Api/Order/{id}`).
    - Get orders for current user (`GET /Api/Order/user` or similar).
  - Implement order status updates (e.g., `Paid`, `Shipped`, `Cancelled`) and related endpoints.
  - Integrate with a payment provider (even if simulated) if you want to practice payment flows.

- **Inventory & Events**
  - Review the MediatR command handlers and tie them into a clear workflow:
    - When products are created/updated, ensure inventory is created/updated accordingly.
    - When stock runs out, consider updating `Product.IsAvailable` automatically.
  - Add background jobs or scheduled tasks if you want more advanced patterns (e.g., re‑sync inventory).

- **Testing & Quality**
  - Add unit tests for services (`AuthService`, `CartService`, `WishListService`, `OrderService`, etc.).
  - Add integration tests for critical flows (register → login → add to cart → place order).
  - Clean up unused imports and small inconsistencies (typos like `ProdcutFilterDto`, `IorderRepository` case, etc.).

- **Deployment & Configuration**
  - Add a simple section describing how to:
    - Run the API locally (`dotnet run`).
    - Configure connection strings for SQLite, MongoDB, and Redis (`appsettings.json`).
  - Add environment‑specific configuration notes (e.g., development vs production).

---

## Quick start (local)

1. **Requirements**
   - .NET 9 SDK
   - Running instances of:
     - SQLite (file DB is configured via `app.db`).
     - MongoDB (as configured in `appsettings.json` under `MongoDb`).
     - Redis (as configured in the `Redis` connection string).

2. **Run the API**
   ```bash
   cd ECommerceApi
   dotnet restore
   dotnet ef database update   # apply EF migrations to SQLite
   dotnet run
   ```

3. **Explore**
   - Browse Swagger UI at the URL printed in the console (usually `https://localhost:5001/swagger` or similar).
   - Use Swagger to:
     - Register a user, login, copy the JWT, and authorize.
     - Test product, cart, wishlist, and order endpoints.

This README is meant to be a living document; as you add new features, update the **What is implemented** and **What is left** sections to match the current state of the project.


