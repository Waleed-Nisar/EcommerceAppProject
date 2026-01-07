using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents an item in the shopping cart
/// </summary>
public class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    [Required]
    public int CartId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CartId))]
    public virtual ShoppingCart ShoppingCart { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    // Computed properties
    [NotMapped]
    public decimal Subtotal => (Product?.Price ?? 0) * Quantity;
}