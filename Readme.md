# E-Commerce Application

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


```
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


```



