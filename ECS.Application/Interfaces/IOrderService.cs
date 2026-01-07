using ECS.Application.DTOs.Common;
using ECS.Application.DTOs.Orders;
using ECS.Domain.Enums;

namespace ECS.Application.Interfaces;

/// <summary>
/// Order service interface
/// </summary>
public interface IOrderService
{
    Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(string userId);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int orderId, string userId, bool isAdmin);
    Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto, string userId);
    Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task<ApiResponse<bool>> CancelOrderAsync(int orderId, string userId);
    Task<ApiResponse<IEnumerable<OrderDto>>> GetAllOrdersAsync();
    Task<ApiResponse<decimal>> GetTotalSalesAsync(DateTime? startDate, DateTime? endDate);
}