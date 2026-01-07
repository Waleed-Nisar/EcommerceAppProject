using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECS.Domain.Enums;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents a payment transaction for an order
/// </summary>
public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    [MaxLength(100)]
    public string TransactionId { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(200)]
    public string? PaymentGatewayResponse { get; set; }

    // Navigation properties
    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;
}