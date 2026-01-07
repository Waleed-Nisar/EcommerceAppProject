using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents a product in the e-commerce system
/// </summary>
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [MaxLength(100)]
    public string? SKU { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [MaxLength(450)]
    public string? CreatedByUserId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    // Computed properties
    [NotMapped]
    public bool IsLowStock => StockQuantity < 10;

    [NotMapped]
    public bool IsOutOfStock => StockQuantity <= 0;
}