using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECS.Domain.Enums;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents a customer order
/// </summary>
public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingCost { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Required]
    [MaxLength(500)]
    public string ShippingAddress { get; set; } = string.Empty;

    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? ShippedDate { get; set; }

    public DateTime? DeliveredDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    // Computed properties
    [NotMapped]
    public decimal Subtotal => TotalAmount - TaxAmount - ShippingCost;

    [NotMapped]
    public bool CanBeCancelled => Status == OrderStatus.Pending || Status == OrderStatus.Processing;
}