using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class CategoryService
{
    private readonly AppDbContext _categoryDbContext;

    public CategoryService(AppDbContext categoryDbContext)
    {
        _categoryDbContext = categoryDbContext;
    }


    public async Task<PaginationResult<Category>> GetAllCategoryService(int currentPage, int pageSize)
    {
        var totalCategoryCount = await _categoryDbContext.Categories.CountAsync();
        var category = await _categoryDbContext.Categories
            .Include(c => c.Products)
            .ThenInclude(r => r.Reviews)
            .Include(p => p.Products)
            .ThenInclude(o => o.OrderProducts)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginationResult<Category>
        {
            Items = category,
            TotalCount = totalCategoryCount,
            CurrentPage = currentPage,
            PageSize = pageSize,
        };
    }


    public async Task<Category?> GetCategoryById(Guid categoryId)
    {
        return await _categoryDbContext.Categories
            .Include(c => c.Products)
            .ThenInclude(r => r.Reviews)
            .Include(p => p.Products)
            .ThenInclude(o => o.OrderProducts)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }


    public async Task<Category> CreateCategoryService(Category newCategory)
    {
        newCategory.CategoryId = Guid.NewGuid();
        newCategory.Slug = SlugGenerator.GenerateSlug(newCategory.Name);
        newCategory.CreatedAt = DateTime.UtcNow;
        _categoryDbContext.Categories.Add(newCategory);
        await _categoryDbContext.SaveChangesAsync();
        return newCategory;
    }


    public async Task<Category?> UpdateCategoryService(Guid categoryId, CategoryDto updateCategory)
    {
        var existingCategory = await _categoryDbContext.Categories.FindAsync(categoryId);
        if (existingCategory != null)
        {
            existingCategory.Name = updateCategory.Name.IsNullOrEmpty() ? existingCategory.Name : updateCategory.Name;
            existingCategory.Slug = SlugGenerator.GenerateSlug(existingCategory.Name);
            existingCategory.Description = updateCategory.Description.IsNullOrEmpty() ? existingCategory.Description : updateCategory.Description;
            await _categoryDbContext.SaveChangesAsync();
        }

        return existingCategory;
    }


    public async Task<bool> DeleteCategoryService(Guid categoryId)
    {
        var categoryToRemove = await _categoryDbContext.Categories.FindAsync(categoryId);
        if (categoryToRemove != null)
        {
            _categoryDbContext.Categories.Remove(categoryToRemove);
            await _categoryDbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}