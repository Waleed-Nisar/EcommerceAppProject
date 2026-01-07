namespace ECS.Application.DTOs.Orders;

public class OrderDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}