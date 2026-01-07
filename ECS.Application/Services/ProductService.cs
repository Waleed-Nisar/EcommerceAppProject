using ECS.Application.DTOs.Common;
using ECS.Application.DTOs.Products;
using ECS.Application.Interfaces;
using ECS.Domain.Entities;
using ECS.Infrastructure.Data;
using ECS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECS.Application.Services;

/// <summary>
/// Product service with business logic
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _context;

    public ProductService(IProductRepository productRepository, ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _context = context;
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetActiveProductsAsync();
        var productDtos = products.Select(MapToDto);

        return ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos);
    }

    public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetProductWithDetailsAsync(id);
        if (product == null)
        {
            return ApiResponse<ProductDto>.ErrorResponse("Product not found");
        }

        return ApiResponse<ProductDto>.SuccessResponse(MapToDto(product));
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
        var productDtos = products.Select(MapToDto);

        return ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos);
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllProductsAsync();
        }

        var products = await _productRepository.SearchProductsAsync(searchTerm);
        var productDtos = products.Select(MapToDto);

        return ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos);
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetLowStockProductsAsync()
    {
        var products = await _productRepository.GetLowStockProductsAsync();
        var productDtos = products.Select(MapToDto);

        return ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos);
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto dto, string userId)
    {
        // Validate stock quantity
        if (dto.StockQuantity < 0)
        {
            return ApiResponse<ProductDto>.ErrorResponse("Stock quantity cannot be negative");
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            SKU = dto.SKU,
            ImageUrl = dto.ImageUrl,
            IsActive = true,
            CreatedByUserId = userId
        };

        await _productRepository.AddAsync(product);
        await _context.SaveChangesAsync();

        return ApiResponse<ProductDto>.SuccessResponse(MapToDto(product), "Product created successfully");
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto dto, string userId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return ApiResponse<ProductDto>.ErrorResponse("Product not found");
        }

        // Validate stock quantity
        if (dto.StockQuantity < 0)
        {
            return ApiResponse<ProductDto>.ErrorResponse("Stock quantity cannot be negative");
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
        product.SKU = dto.SKU;
        product.ImageUrl = dto.ImageUrl;
        product.IsActive = dto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        _productRepository.Update(product);
        await _context.SaveChangesAsync();

        return ApiResponse<ProductDto>.SuccessResponse(MapToDto(product), "Product updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return ApiResponse<bool>.ErrorResponse("Product not found");
        }

        // Check if product has active orders
        var hasActiveOrders = await _productRepository.HasActiveOrdersAsync(id);
        if (hasActiveOrders)
        {
            return ApiResponse<bool>.ErrorResponse("Cannot delete product with active orders");
        }

        _productRepository.Remove(product);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Product deleted successfully");
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            SKU = product.SKU,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive,
            IsLowStock = product.IsLowStock,
            IsOutOfStock = product.IsOutOfStock,
            AverageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
            ReviewCount = product.Reviews.Count
        };
    }
}