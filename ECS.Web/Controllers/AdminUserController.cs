using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECS.Web.Controllers;

[Authorize(Roles = UserRole.Admin)]
public class AdminUserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminUserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userViewModels.Add(new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                IsActive = user.IsActive,
                Roles = roles.ToList(),
                CreatedAt = user.CreatedAt
            });
        }

        return View(userViewModels);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["Error"] = "User not found";
            return RedirectToAction(nameof(Index));
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = await _roleManager.Roles.ToListAsync();

        var viewModel = new EditUserViewModel
        {
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            IsActive = user.IsActive,
            CurrentRoles = userRoles.ToList(),
            AllRoles = allRoles.Select(r => r.Name!).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            TempData["Error"] = "User not found";
            return RedirectToAction(nameof(Index));
        }

        user.IsActive = model.IsActive;
        await _userManager.UpdateAsync(user);

        // Update roles
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
        }

        TempData["Success"] = "User updated successfully";
        return RedirectToAction(nameof(Index));
    }
}

public class UserViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class EditUserViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<string> CurrentRoles { get; set; } = new();
    public List<string> AllRoles { get; set; } = new();
    public List<string>? SelectedRoles { get; set; }
}