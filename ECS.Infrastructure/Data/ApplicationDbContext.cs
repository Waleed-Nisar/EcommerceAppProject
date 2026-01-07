using ECS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ECS.Infrastructure.Data;

/// <summary>
/// Main database context for the e-commerce system
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Product Configuration
        builder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.SKU).IsUnique();
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.CategoryId);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Category Configuration
        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Customer Configuration
        builder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // Order Configuration
        builder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.OrderDate);
            entity.HasIndex(e => e.Status);

            entity.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderItem Configuration
        builder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ShoppingCart Configuration
        builder.Entity<ShoppingCart>(entity =>
        {
            entity.HasIndex(e => e.CustomerId).IsUnique();

            entity.HasOne(sc => sc.Customer)
                .WithOne(c => c.ShoppingCart)
                .HasForeignKey<ShoppingCart>(sc => sc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CartItem Configuration
        builder.Entity<CartItem>(entity =>
        {
            entity.HasOne(ci => ci.ShoppingCart)
                .WithMany(sc => sc.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.CartId, e.ProductId }).IsUnique();
        });

        // Payment Configuration
        builder.Entity<Payment>(entity =>
        {
            entity.HasIndex(e => e.TransactionId).IsUnique();
            entity.HasIndex(e => e.OrderId).IsUnique();

            entity.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Review Configuration
        builder.Entity<Review>(entity =>
        {
            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.ProductId, e.UserId });

            entity.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure decimal precision globally
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}