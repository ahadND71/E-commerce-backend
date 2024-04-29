using Microsoft.AspNetCore.Mvc;
using api.Services;

namespace api.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
  private readonly CategoryService _categoryService;
  public CategoryController(CategoryService categoryService)
  {
    _categoryService = categoryService;
  }

  [HttpGet]
  public IActionResult GetAllUsers()
  {
    var categories = _categoryService.GetAllCategoryService();
    return Ok(categories);
  }

  [HttpGet("{categoryId}")]
  public IActionResult GetCategory(string categoryId)
  {
    if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
    {
      return BadRequest("Invalid category ID Format");
    }
    var category = _categoryService.GetCategoryById(categoryIdGuid);
    if (category == null)
    {
      return NotFound();
    }
    else
    {
      return Ok(category);
    }

  }

  [HttpPost]
  public async Task<IActionResult> CreateCategory(Category newCategory)
  {
    var createdCategory = await _categoryService.CreateCategoryService(newCategory);
    return CreatedAtAction(nameof(GetCategory), new { categoryId = createdCategory.CategoryId }, createdCategory);
  }


  [HttpPut("{categoryId}")]
  public IActionResult UpdateCategory(string categoryId, Category updateCategory)
  {
    if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
    {
      return BadRequest("Invalid category ID Format");
    }
    var category = _categoryService.UpdateCategoryService(categoryIdGuid, updateCategory);
    if (category == null)
    {
      return NotFound();
    }
    return Ok(category);
  }


  [HttpDelete("{categoryId}")]
  public async Task<IActionResult> DeleteCategory(string categoryId)
  {
    if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
    {
      return BadRequest("Invalid category ID Format");
    }
    var result = await _categoryService.DeleteCategoryService(categoryIdGuid);
    if (!result)
    {
      return NotFound();
    }
    return NoContent();
  }

}
