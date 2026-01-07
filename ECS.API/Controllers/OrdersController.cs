using ECS.Application.DTOs.Orders;
using ECS.Application.Interfaces;
using ECS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Get current user's orders
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _orderService.GetUserOrdersAsync(userId!);

        return Ok(result);
    }

    /// <summary>
    /// Get all orders (Admin only)
    /// </summary>
    [HttpGet("all")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get order by ID (Owner or Admin)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole(UserRole.Admin);

        var result = await _orderService.GetOrderByIdAsync(id, userId!, isAdmin);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Create a new order (Customer only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = UserRole.Customer)]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _orderService.CreateOrderAsync(dto, userId!);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.OrderId }, result);
    }

    /// <summary>
    /// Cancel an order (Owner only)
    /// </summary>
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _orderService.CancelOrderAsync(id, userId!);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Update order status (Admin only)
    /// </summary>
    [HttpPut("{id}/status")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _orderService.UpdateOrderStatusAsync(id, dto.Status);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get total sales (Admin only)
    /// </summary>
    [HttpGet("sales")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetSales([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var result = await _orderService.GetTotalSalesAsync(startDate, endDate);
        return Ok(result);
    }
}

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
}