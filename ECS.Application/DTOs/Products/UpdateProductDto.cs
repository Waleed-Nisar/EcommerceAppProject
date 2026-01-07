using System.ComponentModel.DataAnnotations;

namespace ECS.Application.DTOs.Products;

public class UpdateProductDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [MaxLength(100)]
    public string? SKU { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
}