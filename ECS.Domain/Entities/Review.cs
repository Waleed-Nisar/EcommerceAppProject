using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Domain.Entities;

/// <summary>
/// Represents a product review by a customer
/// </summary>
public class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsVerifiedPurchase { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
}