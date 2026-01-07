using ECS.Application.DTOs.Auth;

namespace ECS.Application.Interfaces;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto request);
    Task<bool> LogoutAsync(string userId);
    Task<bool> RevokeRefreshTokenAsync(string userId);
}