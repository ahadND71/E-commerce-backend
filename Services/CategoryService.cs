using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class CategoryService
{
    private readonly AppDbContext _dbContext;

    public CategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PaginationResult<Category>> GetAllCategoryService(int currentPage, int pageSize)
    {
        var totalCategoryCount = await _dbContext.Categories.CountAsync();
        var category = await _dbContext.Categories
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
        return await _dbContext.Categories
            .Include(c => c.Products)
            .ThenInclude(r => r.Reviews)
            .Include(p => p.Products)
            .ThenInclude(o => o.OrderProducts)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }


    public async Task<Category> CreateCategoryService(Category newCategory)
    {
        // Check if a category with the same name already exists
        var existingCategory = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == newCategory.Name);

        if (existingCategory != null)
        {
            throw new InvalidOperationException("Category Is Already Exists");
        }

        newCategory.CategoryId = Guid.NewGuid();
        newCategory.Slug = SlugGenerator.GenerateSlug(newCategory.Name);
        newCategory.CreatedAt = DateTime.UtcNow;
        _dbContext.Categories.Add(newCategory);
        await _dbContext.SaveChangesAsync();
        return newCategory;
    }


    public async Task<Category?> UpdateCategoryService(Guid categoryId, CategoryDto updateCategory)
    {
        var existingCategory = await _dbContext.Categories.FindAsync(categoryId);
        if (existingCategory != null)
        {
            existingCategory.Name = updateCategory.Name.IsNullOrEmpty() ? existingCategory.Name : updateCategory.Name;
            existingCategory.Slug = SlugGenerator.GenerateSlug(existingCategory.Name);
            existingCategory.Description = updateCategory.Description.IsNullOrEmpty() ? existingCategory.Description : updateCategory.Description;
            await _dbContext.SaveChangesAsync();
        }

        return existingCategory;
    }


    public async Task<bool> DeleteCategoryService(Guid categoryId)
    {
        var categoryToRemove = await _dbContext.Categories.FindAsync(categoryId);
        if (categoryToRemove != null)
        {
            _dbContext.Categories.Remove(categoryToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}