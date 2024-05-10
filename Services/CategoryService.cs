using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

namespace api.Services;

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


  public async Task<Category?> UpdateCategoryService(Guid categoryId, Category updateCategory)
  {
    var existingCategory = await _categoryDbContext.Categories.FindAsync(categoryId);
    if (existingCategory != null)
    {
      existingCategory.Name = updateCategory.Name ?? existingCategory.Name;
      existingCategory.Description = updateCategory.Description ?? existingCategory.Name;
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