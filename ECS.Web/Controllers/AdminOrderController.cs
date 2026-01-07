using ECS.Application.Interfaces;
using ECS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Web.Controllers;

[Authorize(Roles = UserRole.Admin)]
public class AdminOrderController : Controller
{
    private readonly IOrderService _orderService;

    public AdminOrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _orderService.GetAllOrdersAsync();
        var orders = result.Data ?? new List<Application.DTOs.Orders.OrderDto>();

        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _orderService.GetOrderByIdAsync(id, "", true);

        if (!result.Success)
        {
            TempData["Error"] = "Order not found";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        var result = await _orderService.UpdateOrderStatusAsync(id, status);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = "Order status updated successfully" });
    }
}