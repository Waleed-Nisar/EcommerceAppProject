using ECS.Application.Interfaces;
using ECS.Domain.Entities;
using ECS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public CategoriesController(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    /// <summary>
    /// Get all categories (Public)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get category by ID (Public)
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get products in a category (Public)
    /// </summary>
    [HttpGet("{id}/products")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryProducts(int id)
    {
        var result = await _productService.GetProductsByCategoryAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Create a new category (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.CreateCategoryAsync(category);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.CategoryId }, result);
    }

    /// <summary>
    /// Update a category (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.UpdateCategoryAsync(id, category);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete a category (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}