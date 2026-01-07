using ECS.Application.DTOs.Common;
using ECS.Application.DTOs.Orders;
using ECS.Application.Interfaces;
using ECS.Domain.Entities;
using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using ECS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECS.Application.Services;

/// <summary>
/// Order service with business logic
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _context;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ApplicationDbContext context)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _context = context;
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(string userId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<IEnumerable<OrderDto>>.ErrorResponse("Customer not found");
        }

        var orders = await _orderRepository.GetCustomerOrdersAsync(customer.CustomerId);
        var orderDtos = orders.Select(MapToDto);

        return ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orderDtos);
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int orderId, string userId, bool isAdmin)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
        if (order == null)
        {
            return ApiResponse<OrderDto>.ErrorResponse("Order not found");
        }

        // Check ownership
        if (!isAdmin)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null || order.CustomerId != customer.CustomerId)
            {
                return ApiResponse<OrderDto>.ErrorResponse("Unauthorized access to order");
            }
        }

        return ApiResponse<OrderDto>.SuccessResponse(MapToDto(order));
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto, string userId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<OrderDto>.ErrorResponse("Customer not found");
        }

        // Validate stock availability
        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                return ApiResponse<OrderDto>.ErrorResponse($"Product {item.ProductId} not found");
            }

            if (product.StockQuantity < item.Quantity)
            {
                return ApiResponse<OrderDto>.ErrorResponse($"Insufficient stock for {product.Name}");
            }
        }

        // Create order
        var order = new Order
        {
            OrderNumber = await _orderRepository.GenerateOrderNumberAsync(),
            CustomerId = customer.CustomerId,
            Status = OrderStatus.Pending,
            ShippingAddress = dto.ShippingAddress,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Notes = dto.Notes,
            OrderDate = DateTime.UtcNow
        };

        // Add order items and calculate totals
        decimal subtotal = 0;
        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product!.Price,
                Discount = 0
            };

            order.OrderItems.Add(orderItem);
            subtotal += orderItem.Subtotal;

            // Reduce stock
            product.StockQuantity -= item.Quantity;
            _productRepository.Update(product);
        }

        order.TaxAmount = subtotal * 0.1m; // 10% tax
        order.ShippingCost = 10.00m;
        order.TotalAmount = subtotal + order.TaxAmount + order.ShippingCost;

        await _orderRepository.AddAsync(order);
        await _context.SaveChangesAsync();

        var createdOrder = await _orderRepository.GetOrderWithDetailsAsync(order.OrderId);
        return ApiResponse<OrderDto>.SuccessResponse(MapToDto(createdOrder!), "Order created successfully");
    }

    public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
        if (order == null)
        {
            return ApiResponse<OrderDto>.ErrorResponse("Order not found");
        }

        order.Status = status;

        if (status == OrderStatus.Shipped)
        {
            order.ShippedDate = DateTime.UtcNow;
        }
        else if (status == OrderStatus.Delivered)
        {
            order.DeliveredDate = DateTime.UtcNow;
        }

        _orderRepository.Update(order);
        await _context.SaveChangesAsync();

        return ApiResponse<OrderDto>.SuccessResponse(MapToDto(order), "Order status updated successfully");
    }

    public async Task<ApiResponse<bool>> CancelOrderAsync(int orderId, string userId)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
        if (order == null)
        {
            return ApiResponse<bool>.ErrorResponse("Order not found");
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null || order.CustomerId != customer.CustomerId)
        {
            return ApiResponse<bool>.ErrorResponse("Unauthorized");
        }

        if (!order.CanBeCancelled)
        {
            return ApiResponse<bool>.ErrorResponse("Order cannot be cancelled at this stage");
        }

        order.Status = OrderStatus.Cancelled;

        // Restore stock
        foreach (var item in order.OrderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.StockQuantity += item.Quantity;
                _productRepository.Update(product);
            }
        }

        _orderRepository.Update(order);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Order cancelled successfully");
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetRecentOrdersAsync(100);
        var orderDtos = orders.Select(MapToDto).ToList();

        return ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orderDtos);
    }

    public async Task<ApiResponse<decimal>> GetTotalSalesAsync(DateTime? startDate, DateTime? endDate)
    {
        var totalSales = await _orderRepository.GetTotalSalesAsync(startDate, endDate);
        return ApiResponse<decimal>.SuccessResponse(totalSales);
    }

    private OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            OrderId = order.OrderId,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.FullName,
            TotalAmount = order.TotalAmount,
            TaxAmount = order.TaxAmount,
            ShippingCost = order.ShippingCost,
            Status = order.Status.ToString(),
            ShippingAddress = order.ShippingAddress,
            City = order.City,
            PostalCode = order.PostalCode,
            Country = order.Country,
            OrderDate = order.OrderDate,
            ShippedDate = order.ShippedDate,
            DeliveredDate = order.DeliveredDate,
            Notes = order.Notes,
            Items = order.OrderItems.Select(oi => new OrderItemDto
            {
                OrderItemId = oi.OrderItemId,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Discount = oi.Discount,
                Subtotal = oi.Subtotal
            }).ToList()
        };
    }
}