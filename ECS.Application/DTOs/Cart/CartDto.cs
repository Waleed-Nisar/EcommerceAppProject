namespace ECS.Application.DTOs.Cart;

public class CartDto
{
    public int CartId { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public string? ImageUrl { get; set; }
}