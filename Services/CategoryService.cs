using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

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
    // List<Category> categories = new List<Category>();
    // var dataList = await _dbContext.Categories.ToListAsync();
    // dataList.ForEach(row => categories.Add(new Category{
    //   CategoryId = row.CategoryId,
    //   Name = row.Name,
    //   Slug = row.Slug,
    //   Description = row.Description,
    //   CreatedAt = row.CreatedAt
    // }));
    // return categories;
    return await _dbContext.Categories.ToListAsync();
  }


  public async Task<Category?> GetCategoryById(Guid categoryId)
  {
    return await _dbContext.Categories.FindAsync(categoryId);
  }


  public async Task<Category> CreateCategoryService(Category newCategory)
  {
    // var category = new Category{
    //   CategoryId = newCategory.CategoryId = Guid.NewGuid(),
    //   Name = newCategory.Name,
    //   Slug = newCategory.Slug = SlugGenerator.GenerateSlug(newCategory.Name),
    //   Description = newCategory.Description,
    //   CreatedAt = DateTime.UtcNow
    // };
    // _dbContext.Categories.Add(newCategory);
    // await _dbContext.SaveChangesAsync();

    newCategory.CategoryId = Guid.NewGuid();
    newCategory.Slug = SlugGenerator.GenerateSlug(newCategory.Name);
    newCategory.CreatedAt = DateTime.UtcNow;
    _dbContext.Categories.Add(newCategory);
    await _dbContext.SaveChangesAsync();
    return newCategory;
  }


  public async Task<Category?> UpdateCategoryService(Guid categoryId, Category updateCategory)
  {
    var existingCategory = await _dbContext.Categories.FindAsync(categoryId);
    if (existingCategory != null)
    {
      existingCategory.Name = updateCategory.Name ?? existingCategory.Name;
      existingCategory.Description = updateCategory.Description ?? existingCategory.Name;
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