using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECS.Domain.Entities;

/// <summary>
/// Extended customer profile linked to ApplicationUser
/// </summary>
public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? ShippingAddress { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ShoppingCart? ShoppingCart { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}".Trim();
}