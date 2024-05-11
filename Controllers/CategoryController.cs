using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;

namespace Backend.Controllers;


[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
  private readonly CategoryService _categoryService;
  public CategoryController(CategoryService categoryService)
  {
    _categoryService = categoryService;
  }


  [AllowAnonymous]
  [HttpGet]
  public async Task<IActionResult> GetAllCategories([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
  {
    var categories = await _categoryService.GetAllCategoryService(currentPage, pageSize);
    if (categories.TotalCount < 1)
    {
      return ApiResponse.NotFound("No Categories To Display");
    }

    return ApiResponse.Success(
              categories,
             "Categories are returned successfully");
  }


  [AllowAnonymous]
  [HttpGet("{categoryId:guid}")]
  public async Task<IActionResult> GetCategory(Guid categoryId)
  {
    var category = await _categoryService.GetCategoryById(categoryId);
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


  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<IActionResult> CreateCategory(Category newCategory)
  {
    var createdCategory = await _categoryService.CreateCategoryService(newCategory);
    if (createdCategory != null)
    {
      return ApiResponse.Created<Category>(createdCategory, "Category is created successfully");
    }
    else
    {
      return ApiResponse.ServerError("Error when creating new category");
    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{categoryId}")]
  public async Task<IActionResult> UpdateCategory(string categoryId, CategoryDto updateCategory)
  {
    if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
    {
      return ApiResponse.BadRequest("Invalid Category ID Format");
    }

    var category = await _categoryService.UpdateCategoryService(categoryIdGuid, updateCategory);
    if (category == null)
    {
      return ApiResponse.NotFound("No Category Founded To Update");

    }
    return ApiResponse.Success<Category>(
              category,
              "Category Is Updated Successfully"
          );
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{categoryId}")]
  public async Task<IActionResult> DeleteCategory(string categoryId)
  {
    if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
    {
      return ApiResponse.BadRequest("Invalid category ID Format");
    }

    var result = await _categoryService.DeleteCategoryService(categoryIdGuid);
    if (!result)
    {
      return ApiResponse.NotFound("The Category is not found to be deleted");
    }
    return ApiResponse.Success("Category is deleted successfully");
  }
}