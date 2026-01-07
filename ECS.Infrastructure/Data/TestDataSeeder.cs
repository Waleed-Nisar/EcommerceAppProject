using ECS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECS.Infrastructure.Data;

/// <summary>
/// Seeds test data (Products, Categories) - ONLY runs in Development environment
/// </summary>
public static class TestDataSeeder
{
    /// <summary>
    /// Seeds sample products and categories for testing
    /// </summary>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (await context.Categories.AnyAsync())
        {
            return; // Database already seeded
        }

        // Seed Categories
        var categories = new List<Category>
        {
            new Category { Name = "Electronics", Description = "Electronic devices and accessories", IsActive = true },
            new Category { Name = "Clothing", Description = "Men's and women's apparel", IsActive = true },
            new Category { Name = "Books", Description = "Physical and digital books", IsActive = true },
            new Category { Name = "Home & Garden", Description = "Home improvement and gardening supplies", IsActive = true },
            new Category { Name = "Sports & Outdoors", Description = "Sports equipment and outdoor gear", IsActive = true }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Seed Products
        var products = new List<Product>
        {
            // Electronics
            new Product
            {
                Name = "Laptop Pro 15",
                Description = "High-performance laptop with 16GB RAM and 512GB SSD",
                Price = 1299.99m,
                StockQuantity = 25,
                CategoryId = categories[0].CategoryId,
                SKU = "ELEC-LAP-001",
                ImageUrl = "https://via.placeholder.com/300x300?text=Laptop",
                IsActive = true
            },
            new Product
            {
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with precision tracking",
                Price = 29.99m,
                StockQuantity = 150,
                CategoryId = categories[0].CategoryId,
                SKU = "ELEC-MOU-002",
                ImageUrl = "https://via.placeholder.com/300x300?text=Mouse",
                IsActive = true
            },
            new Product
            {
                Name = "USB-C Hub",
                Description = "7-in-1 USB-C hub with HDMI and card reader",
                Price = 49.99m,
                StockQuantity = 8, // Low stock
                CategoryId = categories[0].CategoryId,
                SKU = "ELEC-HUB-003",
                ImageUrl = "https://via.placeholder.com/300x300?text=USB-Hub",
                IsActive = true
            },

            // Clothing
            new Product
            {
                Name = "Classic T-Shirt",
                Description = "100% cotton comfortable t-shirt",
                Price = 19.99m,
                StockQuantity = 200,
                CategoryId = categories[1].CategoryId,
                SKU = "CLTH-TSH-001",
                ImageUrl = "https://via.placeholder.com/300x300?text=T-Shirt",
                IsActive = true
            },
            new Product
            {
                Name = "Denim Jeans",
                Description = "Slim fit denim jeans",
                Price = 59.99m,
                StockQuantity = 75,
                CategoryId = categories[1].CategoryId,
                SKU = "CLTH-JNS-002",
                ImageUrl = "https://via.placeholder.com/300x300?text=Jeans",
                IsActive = true
            },

            // Books
            new Product
            {
                Name = "Clean Code",
                Description = "A Handbook of Agile Software Craftsmanship",
                Price = 39.99m,
                StockQuantity = 50,
                CategoryId = categories[2].CategoryId,
                SKU = "BOOK-PGM-001",
                ImageUrl = "https://via.placeholder.com/300x300?text=Book",
                IsActive = true
            },
            new Product
            {
                Name = "Design Patterns",
                Description = "Elements of Reusable Object-Oriented Software",
                Price = 44.99m,
                StockQuantity = 5, // Low stock
                CategoryId = categories[2].CategoryId,
                SKU = "BOOK-PGM-002",
                ImageUrl = "https://via.placeholder.com/300x300?text=Book",
                IsActive = true
            },

            // Home & Garden
            new Product
            {
                Name = "LED Desk Lamp",
                Description = "Adjustable LED desk lamp with USB charging",
                Price = 34.99m,
                StockQuantity = 60,
                CategoryId = categories[3].CategoryId,
                SKU = "HOME-LMP-001",
                ImageUrl = "https://via.placeholder.com/300x300?text=Lamp",
                IsActive = true
            },

            // Sports & Outdoors
            new Product
            {
                Name = "Yoga Mat",
                Description = "Non-slip yoga mat with carrying strap",
                Price = 24.99m,
                StockQuantity = 100,
                CategoryId = categories[4].CategoryId,
                SKU = "SPRT-YGA-001",
                ImageUrl = "https://via.placeholder.com/300x300?text=Yoga-Mat",
                IsActive = true
            },
            new Product
            {
                Name = "Water Bottle",
                Description = "Insulated stainless steel water bottle 750ml",
                Price = 19.99m,
                StockQuantity = 3, // Low stock
                CategoryId = categories[4].CategoryId,
                SKU = "SPRT-BTL-002",
                ImageUrl = "https://via.placeholder.com/300x300?text=Bottle",
                IsActive = true
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}