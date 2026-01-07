using ECS.Application.DTOs.Products;
using ECS.Domain.Entities;

namespace ECS.Application.ViewModels;

/// <summary>
/// View model for product management in admin panel
/// </summary>
public class ProductViewModel
{
    public ProductDto? Product { get; set; }
    public List<Category> Categories { get; set; } = new();
    public List<ProductDto> Products { get; set; } = new();
    public string SearchTerm { get; set; } = string.Empty;
}