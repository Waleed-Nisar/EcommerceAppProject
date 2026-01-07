using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents a customer's shopping cart
/// </summary>
public class ShoppingCart
{
    [Key]
    public int CartId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    // Computed properties
    [NotMapped]
    public decimal TotalAmount => CartItems.Sum(item => item.Subtotal);

    [NotMapped]
    public int TotalItems => CartItems.Sum(item => item.Quantity);

    [NotMapped]
    public bool IsExpired => UpdatedAt < DateTime.UtcNow.AddHours(-24);
}