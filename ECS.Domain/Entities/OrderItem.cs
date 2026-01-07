using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents an item within an order
/// </summary>
public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    // Navigation properties
    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    // Computed properties
    [NotMapped]
    public decimal Subtotal => (UnitPrice * Quantity) - Discount;
}