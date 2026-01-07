using ECS.Application.DTOs.Products;
using ECS.Application.Interfaces;
using ECS.Application.ViewModels;
using ECS.Domain.Enums;
using ECS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECS.Web.Controllers;

[Authorize(Roles = $"{UserRole.Admin},{UserRole.Seller}")]
public class AdminProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ApplicationDbContext _context;

    public AdminProductController(
        IProductService productService,
        ICategoryService categoryService,
        ApplicationDbContext context)
    {
        _productService = productService;
        _categoryService = categoryService;
        _context = context;
    }

    public async Task<IActionResult> Index(string searchTerm = "")
    {
        var viewModel = new ProductViewModel
        {
            SearchTerm = searchTerm
        };

        var result = string.IsNullOrEmpty(searchTerm)
            ? await _productService.GetAllProductsAsync()
            : await _productService.SearchProductsAsync(searchTerm);

        viewModel.Products = result.Data?.ToList() ?? new();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new ProductViewModel();
        var categoriesResult = await _categoryService.GetAllCategoriesAsync();
        viewModel.Categories = categoriesResult.Data?.ToList() ?? new();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new ProductViewModel();
            var categoriesResult = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = categoriesResult.Data?.ToList() ?? new();
            return View(viewModel);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _productService.CreateProductAsync(dto, userId!);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Create));
        }

        TempData["Success"] = "Product created successfully";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var productResult = await _productService.GetProductByIdAsync(id);
        if (!productResult.Success)
        {
            TempData["Error"] = "Product not found";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new ProductViewModel
        {
            Product = productResult.Data
        };

        var categoriesResult = await _categoryService.GetAllCategoriesAsync();
        viewModel.Categories = categoriesResult.Data?.ToList() ?? new();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new ProductViewModel();
            var categoriesResult = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = categoriesResult.Data?.ToList() ?? new();
            return View(viewModel);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _productService.UpdateProductAsync(id, dto, userId!);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Edit), new { id });
        }

        TempData["Success"] = "Product updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = "Product deleted successfully" });
    }
}