using ECS.Application.DTOs.Common;
using ECS.Application.DTOs.Products;

namespace ECS.Application.Interfaces;

/// <summary>
/// Product service interface
/// </summary>
public interface IProductService
{
    Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
    Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
    Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(int categoryId);
    Task<ApiResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm);
    Task<ApiResponse<IEnumerable<ProductDto>>> GetLowStockProductsAsync();
    Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto dto, string userId);
    Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto dto, string userId);
    Task<ApiResponse<bool>> DeleteProductAsync(int id);
}