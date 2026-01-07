using ECS.Application.DTOs.Cart;
using ECS.Application.DTOs.Common;
using ECS.Application.Interfaces;
using ECS.Domain.Entities;
using ECS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECS.Application.Services;

/// <summary>
/// Shopping cart service with business logic
/// </summary>
public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CartDto>> GetCartAsync(string userId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<CartDto>.ErrorResponse("Customer not found");
        }

        var cart = await _context.ShoppingCarts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        if (cart == null)
        {
            // Create new cart
            cart = new ShoppingCart
            {
                CustomerId = customer.CustomerId
            };
            _context.ShoppingCarts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Remove expired items (older than 24 hours)
        var expiredItems = cart.CartItems.Where(ci => ci.AddedAt < DateTime.UtcNow.AddHours(-24)).ToList();
        if (expiredItems.Any())
        {
            _context.CartItems.RemoveRange(expiredItems);
            await _context.SaveChangesAsync();
        }

        return ApiResponse<CartDto>.SuccessResponse(MapToDto(cart));
    }

    public async Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto dto)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<CartDto>.ErrorResponse("Customer not found");
        }

        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
        {
            return ApiResponse<CartDto>.ErrorResponse("Product not found");
        }

        if (product.StockQuantity < dto.Quantity)
        {
            return ApiResponse<CartDto>.ErrorResponse("Insufficient stock");
        }

        var cart = await _context.ShoppingCarts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        if (cart == null)
        {
            cart = new ShoppingCart { CustomerId = customer.CustomerId };
            _context.ShoppingCarts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Check if product already in cart
        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
            existingItem.AddedAt = DateTime.UtcNow;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            cart.CartItems.Add(cartItem);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<CartDto>.SuccessResponse(MapToDto(cart), "Item added to cart");
    }

    public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, int cartItemId, int quantity)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<CartDto>.ErrorResponse("Customer not found");
        }

        var cartItem = await _context.CartItems
            .Include(ci => ci.ShoppingCart)
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

        if (cartItem == null || cartItem.ShoppingCart.CustomerId != customer.CustomerId)
        {
            return ApiResponse<CartDto>.ErrorResponse("Cart item not found");
        }

        if (quantity <= 0)
        {
            // Remove item and return updated cart
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            var cartAfterRemoval = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartItem.CartId);

            return ApiResponse<CartDto>.SuccessResponse(MapToDto(cartAfterRemoval!), "Item removed from cart");
        }

        if (cartItem.Product.StockQuantity < quantity)
        {
            return ApiResponse<CartDto>.ErrorResponse("Insufficient stock");
        }

        cartItem.Quantity = quantity;
        cartItem.ShoppingCart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var cart = await _context.ShoppingCarts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.CartId == cartItem.CartId);

        return ApiResponse<CartDto>.SuccessResponse(MapToDto(cart!), "Cart updated");
    }

    public async Task<ApiResponse<bool>> RemoveFromCartAsync(string userId, int cartItemId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<bool>.ErrorResponse("Customer not found");
        }

        var cartItem = await _context.CartItems
            .Include(ci => ci.ShoppingCart)
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

        if (cartItem == null || cartItem.ShoppingCart.CustomerId != customer.CustomerId)
        {
            return ApiResponse<bool>.ErrorResponse("Cart item not found");
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Item removed from cart");
    }

    public async Task<ApiResponse<bool>> ClearCartAsync(string userId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            return ApiResponse<bool>.ErrorResponse("Customer not found");
        }

        var cart = await _context.ShoppingCarts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        if (cart != null)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }

        return ApiResponse<bool>.SuccessResponse(true, "Cart cleared");
    }

    private CartDto MapToDto(ShoppingCart cart)
    {
        return new CartDto
        {
            CartId = cart.CartId,
            TotalAmount = cart.TotalAmount,
            TotalItems = cart.TotalItems,
            Items = cart.CartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.CartItemId,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                ProductPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity,
                Subtotal = ci.Subtotal,
                ImageUrl = ci.Product?.ImageUrl
            }).ToList()
        };
    }
}