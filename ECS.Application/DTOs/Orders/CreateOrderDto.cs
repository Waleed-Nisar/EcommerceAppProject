using System.ComponentModel.DataAnnotations;

namespace ECS.Application.DTOs.Orders;

public class CreateOrderDto
{
    [Required]
    [MaxLength(500)]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public class CreateOrderItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}