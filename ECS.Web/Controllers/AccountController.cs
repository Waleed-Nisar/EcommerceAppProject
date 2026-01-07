using ECS.Infrastructure.Data;
using ECS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECS.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    /*
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Check if user has admin or seller role
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin") && !roles.Contains("Seller"))
            {
                await _signInManager.SignOutAsync();
                ModelState.AddModelError(string.Empty, "Access denied. Admin or Seller role required.");
                return View(model);
            }

            return RedirectToLocal(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return View(model);
    }*/
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError(string.Empty, "Email not confirmed.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Admin") && !roles.Contains("Seller"))
        {
            await _signInManager.SignOutAsync();
            ModelState.AddModelError(string.Empty, "Access denied. Admin or Seller role required.");
            return View(model);
        }

        return RedirectToLocal(returnUrl);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Index", "Home");
    }
}