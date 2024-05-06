using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using api.Authentication.Identity;

namespace api.Controllers;
[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{

  private readonly CategoryService _dbContext;
  public CategoryController(CategoryService categoryService)
  {
    _dbContext = categoryService;
  }


  [AllowAnonymous]
  [HttpGet]
  public async Task<IActionResult> GetAllCategories()
  {
    try
    {
      var categories = await _dbContext.GetAllCategoryService();

      if (categories.ToList().Count < 1)
      {
        return ApiResponse.NotFound("No Categories To Display");

      }
      return ApiResponse.Success<IEnumerable<Category>>(
                categories,
               "Categories are returned successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the category list");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [AllowAnonymous]
  [HttpGet("{categoryId:guid}")]
  public async Task<IActionResult> GetCategory(Guid categoryId)
  {
    try
    {
      var category = await _dbContext.GetCategoryById(categoryId);
      if (category == null)
      {
        return ApiResponse.NotFound(
                 $"No Category Found With ID : ({categoryId})");
      }
      else
      {
        return ApiResponse.Success<Category>(
                   category,
                   "Category is returned successfully"
                 );
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the category");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize]
  [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
  [HttpPost]
  public async Task<IActionResult> CreateCategory(Category newCategory)
  {
    try
    {
      var createdCategory = await _dbContext.CreateCategoryService(newCategory);
      if (createdCategory != null)
      {
        return ApiResponse.Created<Category>(createdCategory, "Category is created successfully");
      }
      else
      {
        return ApiResponse.ServerError("Error when creating new category");

      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot create new category");
      return ApiResponse.ServerError(ex.Message);

    }
  }

  [Authorize]
  [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
  [HttpPut("{categoryId}")]
  public async Task<IActionResult> UpdateCategory(string categoryId, Category updateCategory)
  {
    try
    {
      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return ApiResponse.BadRequest("Invalid Category ID Format");
      }
      var category = await _dbContext.UpdateCategoryService(categoryIdGuid, updateCategory);
      if (category == null)
      {
        return ApiResponse.NotFound("No Category Founded To Update");

      }
      return ApiResponse.Success<Category>(
                category,
                "Category Is Updated Successfully"
            );
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot update the category");
      return ApiResponse.ServerError(ex.Message);

    }
  }

  [Authorize]
  [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
  [HttpDelete("{categoryId}")]
  public async Task<IActionResult> DeleteCategory(string categoryId)
  {
    try
    {
      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return ApiResponse.BadRequest("Invalid category ID Format");
      }
      var result = await _dbContext.DeleteCategoryService(categoryIdGuid);
      if (!result)
      {
        return ApiResponse.NotFound("The Category is not found to be deleted");
      }
      return ApiResponse.Success("Category is deleted successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, the category can not deleted");
      return ApiResponse.ServerError(ex.Message);

    }
  }
}
