using ECS.Domain.Entities;

namespace ECS.Infrastructure.Repositories;

/// <summary>
/// Product-specific repository interface with specialized queries
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<Product?> GetProductWithDetailsAsync(int productId);
    Task<bool> HasActiveOrdersAsync(int productId);
}