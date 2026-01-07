using ECS.Application.DTOs.Cart;
using ECS.Application.DTOs.Common;

namespace ECS.Application.Interfaces;

/// <summary>
/// Shopping cart service interface
/// </summary>
public interface ICartService
{
    Task<ApiResponse<CartDto>> GetCartAsync(string userId);
    Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto dto);
    Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, int cartItemId, int quantity);
    Task<ApiResponse<bool>> RemoveFromCartAsync(string userId, int cartItemId);
    Task<ApiResponse<bool>> ClearCartAsync(string userId);
}