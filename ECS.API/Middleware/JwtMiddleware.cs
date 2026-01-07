using ECS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace ECS.API.Middleware;

/// <summary>
/// Middleware for JWT token validation and user attachment
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContext(context, userManager, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, UserManager<ApplicationUser> userManager, string token)
    {
        try
        {
            // Token validation is handled by ASP.NET Core JWT Bearer authentication
            // This middleware can be used for additional custom logic if needed

            // Example: Check if user is still active
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user == null || !user.IsActive)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("User account is inactive");
                        return;
                    }
                }
            }
        }
        catch
        {
            // Token validation failed - do nothing, let the request continue
        }
    }
}