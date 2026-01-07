namespace ECS.Domain.Enums;

/// <summary>
/// Defines user roles for authorization
/// </summary>
public static class UserRole
{
    public const string Admin = "Admin";
    public const string Seller = "Seller";
    public const string Customer = "Customer";
    public const string Guest = "Guest";

    /// <summary>
    /// Gets all role names as a collection
    /// </summary>
    public static IEnumerable<string> GetAllRoles()
    {
        return new[] { Admin, Seller, Customer, Guest };
    }

    /// <summary>
    /// Checks if a role name is valid
    /// </summary>
    public static bool IsValid(string role)
    {
        return GetAllRoles().Contains(role);
    }
}