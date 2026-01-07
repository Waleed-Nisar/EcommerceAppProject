using ECS.Domain.Entities;
using ECS.Domain.Enums;

namespace ECS.Infrastructure.Repositories;

/// <summary>
/// Order-specific repository interface with specialized queries
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10);
    Task<string> GenerateOrderNumberAsync();
    Task<decimal> GetTotalSalesAsync(DateTime? startDate = null, DateTime? endDate = null);
}