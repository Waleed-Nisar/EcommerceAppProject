using ECS.Application.Interfaces;
using ECS.Application.ViewModels;
using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECS.Web.Controllers;

[Authorize(Roles = $"{UserRole.Admin},{UserRole.Seller}")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public HomeController(ApplicationDbContext context, IProductService productService, IOrderService orderService)
    {
        _context = context;
        _productService = productService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel();

        // Get total sales
        var salesResult = await _orderService.GetTotalSalesAsync(null, null);
        viewModel.TotalSales = salesResult.Data;

        // Get order counts
        viewModel.TotalOrders = await _context.Orders.CountAsync();
        viewModel.PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);

        // Get product counts
        viewModel.TotalProducts = await _context.Products.CountAsync(p => p.IsActive);

        // Get low stock products
        var lowStockResult = await _productService.GetLowStockProductsAsync();
        viewModel.LowStockProducts = lowStockResult.Data?.ToList() ?? new();
        viewModel.LowStockCount = viewModel.LowStockProducts.Count;

        // Get recent orders
        var recentOrdersResult = await _orderService.GetAllOrdersAsync();
        viewModel.RecentOrders = recentOrdersResult.Data?.Take(10).ToList() ?? new();

        return View(viewModel);
    }
}