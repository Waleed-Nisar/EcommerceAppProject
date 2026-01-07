using ECS.Domain.Entities;
using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECS.Infrastructure.Repositories;

/// <summary>
/// Product repository with specialized business queries
/// </summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.StockQuantity > 0)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.StockQuantity < threshold && p.StockQuantity > 0 && p.IsActive)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        var lowerTerm = searchTerm.ToLower();
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.IsActive &&
                (p.Name.ToLower().Contains(lowerTerm) ||
                 p.Description!.ToLower().Contains(lowerTerm) ||
                 p.SKU!.ToLower().Contains(lowerTerm)))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithDetailsAsync(int productId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.ProductId == productId);
    }

    public async Task<bool> HasActiveOrdersAsync(int productId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .AnyAsync(oi => oi.ProductId == productId &&
                           oi.Order.Status != OrderStatus.Cancelled &&
                           oi.Order.Status != OrderStatus.Refunded);
    }
}