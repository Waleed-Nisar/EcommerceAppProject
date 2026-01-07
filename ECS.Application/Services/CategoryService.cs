using ECS.Application.DTOs.Common;
using ECS.Application.Interfaces;
using ECS.Domain.Entities;
using ECS.Infrastructure.Data;
using ECS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECS.Application.Services;

/// <summary>
/// Category service with business logic
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly ApplicationDbContext _context;

    public CategoryService(IRepository<Category> categoryRepository, ApplicationDbContext context)
    {
        _categoryRepository = categoryRepository;
        _context = context;
    }

    public async Task<ApiResponse<IEnumerable<Category>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.FindAsync(c => c.IsActive);
        return ApiResponse<IEnumerable<Category>>.SuccessResponse(categories);
    }

    public async Task<ApiResponse<Category>> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return ApiResponse<Category>.ErrorResponse("Category not found");
        }

        return ApiResponse<Category>.SuccessResponse(category);
    }

    public async Task<ApiResponse<Category>> CreateCategoryAsync(Category category)
    {
        // Check if category name already exists
        var exists = await _categoryRepository.ExistsAsync(c => c.Name == category.Name);
        if (exists)
        {
            return ApiResponse<Category>.ErrorResponse("Category with this name already exists");
        }

        await _categoryRepository.AddAsync(category);
        await _context.SaveChangesAsync();

        return ApiResponse<Category>.SuccessResponse(category, "Category created successfully");
    }

    public async Task<ApiResponse<Category>> UpdateCategoryAsync(int id, Category category)
    {
        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ApiResponse<Category>.ErrorResponse("Category not found");
        }

        existing.Name = category.Name;
        existing.Description = category.Description;
        existing.IsActive = category.IsActive;

        _categoryRepository.Update(existing);
        await _context.SaveChangesAsync();

        return ApiResponse<Category>.SuccessResponse(existing, "Category updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return ApiResponse<bool>.ErrorResponse("Category not found");
        }

        if (category.Products.Any())
        {
            return ApiResponse<bool>.ErrorResponse("Cannot delete category with existing products");
        }

        _categoryRepository.Remove(category);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully");
    }
}