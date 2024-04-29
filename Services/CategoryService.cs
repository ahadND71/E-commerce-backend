using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Services;

public class CategoryService
{

  private readonly AppDbContext _dbContext;
  public CategoryService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<IEnumerable<Category>> GetAllCategoryService()
  {
    return await _dbContext.Categories.ToListAsync();
  }


  public async Task<Category?> GetCategoryById(Guid categoryId)
  {
    return await _dbContext.Categories.FindAsync(categoryId);
  }


  public async Task<Category> CreateCategoryService(Category newCategory)
  {
    newCategory.CategoryId = Guid.NewGuid();
    newCategory.CreatedAt = DateTime.Now;
    _dbContext.Categories.Add(newCategory);
    await _dbContext.SaveChangesAsync();
    return newCategory;
  }


  public async Task<Category?> UpdateCategoryService(Guid categoryId, Category updateCategory)
  {
    var existingCategory = await _dbContext.Categories.FindAsync(categoryId);
    if (existingCategory != null)
    {
      existingCategory.Name = updateCategory.Name;
      existingCategory.Description = updateCategory.Description;
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