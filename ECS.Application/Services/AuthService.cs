using ECS.Application.DTOs.Auth;
using ECS.Application.Interfaces;
using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using ECS.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECS.Application.Services;

/// <summary>
/// Authentication service with JWT token management
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly ApplicationDbContext _context;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        JwtTokenGenerator tokenGenerator,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
        _context = context;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User with this email already exists"
            };
        }

        // Create new user
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        // Assign Customer role by default
        await _userManager.AddToRoleAsync(user, UserRole.Customer);

        // Create customer profile
        var customer = new Domain.Entities.Customer
        {
            UserId = user.Id,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Generate tokens
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenGenerator.GenerateAccessToken(user, roles);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        // Save refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = _tokenGenerator.GetRefreshTokenExpiration();
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            Roles = roles.ToList(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            Message = "Registration successful"
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Generate tokens
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenGenerator.GenerateAccessToken(user, roles);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        // Save refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = _tokenGenerator.GetRefreshTokenExpiration();
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            Roles = roles.ToList(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            Message = "Login successful"
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto request)
    {
        var principal = _tokenGenerator.ValidateToken(request.AccessToken);
        if (principal == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid access token"
            };
        }

        var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null || user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid refresh token"
            };
        }

        // Generate new tokens
        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _tokenGenerator.GenerateAccessToken(user, roles);
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

        // Update refresh token
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = _tokenGenerator.GetRefreshTokenExpiration();
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            UserId = user.Id,
            Email = user.Email!,
            Roles = roles.ToList(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            Message = "Token refreshed successfully"
        };
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);

        return true;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string userId)
    {
        return await LogoutAsync(userId);
    }
}