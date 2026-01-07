# E-Commerce System (ECS)

Production-grade E-Commerce REST API with JWT Authentication and Admin Panel built with ASP.NET Core 8.

## 🎯 Features

### Core Functionality
- **JWT Authentication** - Secure token-based authentication with refresh tokens
- **Role-Based Access Control** - Admin, Seller, Customer, Guest roles
- **Product Management** - Full CRUD with search, categories, and stock tracking
- **Shopping Cart** - Session-based cart with 24-hour expiry
- **Order Processing** - Complete order lifecycle with status tracking
- **Product Reviews** - Customer reviews with verified purchase badges
- **Admin Dashboard** - Real-time stats, low stock alerts, order management

### Technical Features
- Clean Architecture (Domain, Infrastructure, Application, API layers)
- Repository Pattern with EF Core
- Smart Seeding Strategy (Environment-aware)
- Global Exception Handling
- API Response Wrappers
- Swagger UI with JWT support
- DataTables for admin panel
- Bootstrap 5 responsive design

## 📋 Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (or SQL Server)
- Visual Studio 2022 or VS Code
- Postman (optional, for API testing)

## 🚀 Quick Start

### 1. Clone and Setup

```bash
# The project structure is already set up with 4 class libraries:
# - ECS.Domain (Entities & Enums)
# - ECS.Infrastructure (Data & Repositories & Security)
# - ECS.Application (Services & DTOs & ViewModels)
# - ECS.API (REST API Controllers)
# - ECS.Web (MVC Admin Panel)
```

### 2. Database Configuration

**Connection String** (already in appsettings.json):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

**Migrations** (Auto-applied on startup):
```bash
# Optional: Manual migration commands
cd ECS.API
dotnet ef migrations add InitialCreate --project ../ECS.Infrastructure
dotnet ef database update --project ../ECS.Infrastructure
```

### 3. Run the API

```bash
cd ECS.API
dotnet run
```

API will be available at: `https://localhost:7001`

Swagger UI: `https://localhost:7001/swagger`

### 4. Run the Admin Panel

```bash
cd ECS.Web
dotnet run
```

Admin Panel: `https://localhost:7002`

## 🔐 Authentication

### JWT Token Flow

1. **Register** → POST `/api/auth/register` (Customer role assigned)
2. **Login** → POST `/api/auth/login` → Receive JWT + Refresh Token
3. **Use Token** → Add `Authorization: Bearer {token}` header
4. **Refresh** → POST `/api/auth/refresh` (when token expires after 15 min)

### Development Credentials

**API & Admin Panel:**
```
Admin:
  Email: admin@ecommerce.com
  Password: Admin@123

Seller:
  Email: seller@ecommerce.com
  Password: Seller@123

Customer:
  Email: customer@ecommerce.com
  Password: Customer@123
```

## 📡 API Endpoints

### Authentication (Public)
```
POST   /api/auth/register      - Register new customer
POST   /api/auth/login         - Login and get JWT token
POST   /api/auth/refresh       - Refresh expired token
POST   /api/auth/logout        - Invalidate refresh token
```

### Products
```
GET    /api/products                    - Get all products (Public)
GET    /api/products/{id}               - Get product by ID (Public)
GET    /api/products/search?q=laptop    - Search products (Public)
GET    /api/products/category/{id}      - Get by category (Public)
GET    /api/products/low-stock          - Low stock alert (Admin/Seller)
POST   /api/products                    - Create product (Admin/Seller)
PUT    /api/products/{id}               - Update product (Admin/Seller)
DELETE /api/products/{id}               - Delete product (Admin only)
```

### Categories
```
GET    /api/categories              - Get all categories (Public)
GET    /api/categories/{id}         - Get category by ID (Public)
GET    /api/categories/{id}/products - Get products in category (Public)
POST   /api/categories              - Create category (Admin)
PUT    /api/categories/{id}         - Update category (Admin)
DELETE /api/categories/{id}         - Delete category (Admin)
```

### Shopping Cart (Authenticated)
```
GET    /api/cart              - Get my cart
POST   /api/cart/items        - Add item to cart
PUT    /api/cart/items/{id}   - Update quantity
DELETE /api/cart/items/{id}   - Remove item
DELETE /api/cart/clear        - Clear entire cart
```

### Orders
```
GET    /api/orders            - Get my orders (Authenticated)
GET    /api/orders/all        - Get all orders (Admin)
GET    /api/orders/{id}       - Get order details (Owner/Admin)
POST   /api/orders            - Create order (Customer)
PUT    /api/orders/{id}/cancel - Cancel order (Owner)
PUT    /api/orders/{id}/status - Update status (Admin)
GET    /api/orders/sales      - Get total sales (Admin)
```

### Reviews
```
GET    /api/reviews/products/{id}  - Get product reviews (Public)
POST   /api/reviews/products/{id}  - Create review (Customer)
DELETE /api/reviews/{id}           - Delete review (Admin/Owner)
```

## 🎭 Role-Based Access

| Endpoint | Guest | Customer | Seller | Admin |
|----------|-------|----------|--------|-------|
| Browse Products | ✅ | ✅ | ✅ | ✅ |
| Add to Cart | ❌ | ✅ | ✅ | ✅ |
| Place Order | ❌ | ✅ | ❌ | ✅ |
| Create Product | ❌ | ❌ | ✅ | ✅ |
| Delete Product | ❌ | ❌ | ❌ | ✅ |
| Manage Users | ❌ | ❌ | ❌ | ✅ |

## 🌱 Smart Seeding Strategy

The system uses environment-aware seeding:

**1. Essential Data (Always runs):**
- Roles (Admin, Seller, Customer, Guest)

**2. Debug Users (DEBUG builds only):**
```csharp
#if DEBUG
await DbSeeder.SeedDebugUsersAsync(userManager, roleManager);
#endif
```
- Admin, Seller, Customer test accounts

**3. Test Data (Development environment only):**
```csharp
if (app.Environment.IsDevelopment())
{
    await TestDataSeeder.SeedAsync(context);
}
```
- 10 sample products
- 5 categories

## 📊 Database Schema

### Key Entities
- **ApplicationUser** - Identity user with refresh token support
- **Customer** - Extended customer profile
- **Product** - Products with stock tracking
- **Category** - Product categories
- **ShoppingCart** - Active carts with 24h expiry
- **Order** - Customer orders with status tracking
- **OrderItem** - Individual line items
- **Payment** - Payment records (mock)
- **Review** - Product reviews with verified purchase flag

## 🛠️ Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY_MINIMUM_32_CHARACTERS",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

**⚠️ Production:** Replace `SecretKey` with a secure 256-bit key (32+ characters)

### CORS Configuration
```csharp
options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});
```

**⚠️ Production:** Restrict to specific origins

## 📦 Project Structure

```
ECommerceSystem/
├── ECS.Domain/
│   ├── Entities/        # Domain models
│   └── Enums/          # Order status, payment methods, roles
├── ECS.Infrastructure/
│   ├── Data/           # DbContext, seeding
│   ├── Repositories/   # Data access layer
│   └── Security/       # JWT token generation
├── ECS.Application/
│   ├── Interfaces/     # Service contracts
│   ├── Services/       # Business logic
│   ├── DTOs/          # Data transfer objects
│   └── ViewModels/    # MVC view models
├── ECS.API/
│   ├── Controllers/    # REST API endpoints
│   ├── Middleware/     # Exception handling, JWT
│   └── Program.cs      # API configuration
└── ECS.Web/
    ├── Controllers/    # MVC controllers
    ├── Views/         # Razor views
    └── wwwroot/       # Static files
```

## 🧪 Testing with Postman

1. Import `ECommerce_API.postman_collection.json`
2. Update `baseUrl` variable to your API URL
3. Login with test credentials
4. Token automatically saved to collection variable
5. Test all endpoints with authentication

## 📈 Admin Dashboard Features

- **Sales Overview** - Total sales, order count, pending orders
- **Low Stock Alerts** - Products below threshold (< 10)
- **Product Management** - Create, edit, delete with DataTables
- **Order Management** - View all orders, update status
- **User Management** - Assign roles, activate/deactivate accounts

## 🔒 Security Features

- JWT Bearer token authentication
- Password hashing (PBKDF2 via Identity)
- Refresh token rotation
- Role-based authorization
- Resource ownership validation
- Anti-forgery tokens (MVC)
- HTTPS enforcement
- SQL injection protection (EF Core parameterization)

## 🚧 Business Rules

1. Products cannot have negative stock
2. Orders cannot be placed for out-of-stock items
3. Only order owner or Admin can view order details
4. Cart items expire after 24 hours
5. Products with active orders cannot be deleted
6. Customers can only review purchased products
7. Low stock alert when quantity < 10

## 📝 API Response Format

```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... },
  "errors": []
}
```

## 🐛 Troubleshooting

**Migration Issues:**
```bash
# Delete database and recreate
dotnet ef database drop --project ECS.Infrastructure
dotnet ef database update --project ECS.Infrastructure
```

**JWT Token Expired:**
- Use refresh token endpoint
- Or login again

**Admin Panel Login Fails:**
- Ensure user has Admin or Seller role
- Check credentials match seeded users

## 📄 License

MIT License - Free to use for learning and commercial purposes

## 👨‍💻 Author

Generated with production-grade best practices for ASP.NET Core 8

---

**🎉 You're ready to build! Run the API, test with Swagger, and explore the Admin Panel.**