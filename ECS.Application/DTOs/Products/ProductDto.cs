namespace ECS.Application.DTOs.Products;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? SKU { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsLowStock { get; set; }
    public bool IsOutOfStock { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}