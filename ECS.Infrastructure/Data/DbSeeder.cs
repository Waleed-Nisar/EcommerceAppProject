using ECS.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ECS.Infrastructure.Data;

/// <summary>
/// Seeds essential data (Roles) - ALWAYS runs in all environments
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Seeds essential roles required for the system to function
    /// </summary>
    public static async Task SeedEssentialDataAsync(RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles (CRITICAL - always needed)
        var roles = UserRole.GetAllRoles();
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

#if DEBUG
    /// <summary>
    /// Seeds test users for development - ONLY in DEBUG builds
    /// </summary>
    public static async Task SeedDebugUsersAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Ensure roles exist
        await SeedEssentialDataAsync(roleManager);

        // Seed Admin User
        var adminEmail = "admin@ecommerce.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserRole.Admin);
            }
        }

        // Seed Seller User
        var sellerEmail = "seller@ecommerce.com";
        if (await userManager.FindByEmailAsync(sellerEmail) == null)
        {
            var sellerUser = new ApplicationUser
            {
                UserName = sellerEmail,
                Email = sellerEmail,
                FirstName = "John",
                LastName = "Seller",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(sellerUser, "Seller@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(sellerUser, UserRole.Seller);
            }
        }

        // Seed Customer User
        var customerEmail = "customer@ecommerce.com";
        if (await userManager.FindByEmailAsync(customerEmail) == null)
        {
            var customerUser = new ApplicationUser
            {
                UserName = customerEmail,
                Email = customerEmail,
                FirstName = "Jane",
                LastName = "Customer",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(customerUser, "Customer@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customerUser, UserRole.Customer);
            }
        }
    }
#endif
}