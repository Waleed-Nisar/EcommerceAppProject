using ECS.Domain.Entities;
using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECS.Infrastructure.Repositories;

/// <summary>
/// Order repository with specialized business queries
/// </summary>
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Payment)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Category)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var date = DateTime.UtcNow;
        var prefix = $"ORD-{date:yyyyMMdd}";

        var lastOrder = await _dbSet
            .Where(o => o.OrderNumber.StartsWith(prefix))
            .OrderByDescending(o => o.OrderNumber)
            .FirstOrDefaultAsync();

        if (lastOrder == null)
        {
            return $"{prefix}-0001";
        }

        var lastNumber = int.Parse(lastOrder.OrderNumber.Split('-').Last());
        return $"{prefix}-{(lastNumber + 1):D4}";
    }

    public async Task<decimal> GetTotalSalesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.Where(o => o.Status != OrderStatus.Cancelled &&
                                      o.Status != OrderStatus.Refunded);

        if (startDate.HasValue)
            query = query.Where(o => o.OrderDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(o => o.OrderDate <= endDate.Value);

        return await query.SumAsync(o => o.TotalAmount);
    }
}