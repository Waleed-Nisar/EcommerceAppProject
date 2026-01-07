using ECS.Application.DTOs.Products;
using ECS.Application.Interfaces;
using ECS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get all active products (Public)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllProductsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get product by ID (Public)
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Search products by name, description, or SKU (Public)
    /// </summary>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var result = await _productService.SearchProductsAsync(q);
        return Ok(result);
    }

    /// <summary>
    /// Get products by category (Public)
    /// </summary>
    [HttpGet("category/{categoryId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _productService.GetProductsByCategoryAsync(categoryId);
        return Ok(result);
    }

    /// <summary>
    /// Get low stock products (Admin/Seller only)
    /// </summary>
    [HttpGet("low-stock")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Seller}")]
    public async Task<IActionResult> GetLowStock()
    {
        var result = await _productService.GetLowStockProductsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Create a new product (Admin/Seller only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Seller}")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _productService.CreateProductAsync(dto, userId!);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ProductId }, result);
    }

    /// <summary>
    /// Update a product (Admin/Seller only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Seller}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _productService.UpdateProductAsync(id, dto, userId!);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete a product (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}