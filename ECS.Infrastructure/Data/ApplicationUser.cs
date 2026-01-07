using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECS.Infrastructure.Data;

/// <summary>
/// Extended IdentityUser with additional properties for e-commerce
/// </summary>
public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Computed property
    public string FullName => $"{FirstName} {LastName}".Trim();
}