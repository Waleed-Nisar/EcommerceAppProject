using ECS.Application.DTOs.Orders;
using ECS.Application.DTOs.Products;

namespace ECS.Application.ViewModels;

/// <summary>
/// View model for admin dashboard
/// </summary>
public class DashboardViewModel
{
    public decimal TotalSales { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int TotalProducts { get; set; }
    public int LowStockCount { get; set; }
    public List<ProductDto> LowStockProducts { get; set; } = new();
    public List<OrderDto> RecentOrders { get; set; } = new();
}