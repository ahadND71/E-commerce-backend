using Microsoft.AspNetCore.Mvc;
using api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using api.Helpers;

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


  [HttpGet]
  public async Task<IActionResult> GetAllCategories()
  {
    try
    {
      var categories = await _dbContext.GetAllCategoryService();

      if (categories.ToList().Count < 1)
      {
        return NotFound(new ErrorMessage
        {
          Message = "No Categories To Display"
        });
      }
      return Ok(new SuccessMessage<IEnumerable<Category>>
      {
        Message = "Categories are returned successfully",
        Data = categories
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the category list");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpGet("{categoryId:guid}")]
  public async Task<IActionResult> GetCategory(Guid categoryId)
  {
    try
    {
      // if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      // {
      //   return BadRequest("Invalid category ID Format");
      // }
      var category = await _dbContext.GetCategoryById(categoryId);
      if (category == null)
      {
        return NotFound(new ErrorMessage
        {
          Message = $"No Category Found With ID : ({categoryId})"
        });
      }
      else
      {
        return Ok(new SuccessMessage<Category>
        {
          Success = true,
          Message = "Category is returned successfully",
          Data = category
        });
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the category");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpPost]
  public async Task<IActionResult> CreateCategory(Category newCategory)
  {
    try
    {
      var createdCategory = await _dbContext.CreateCategoryService(newCategory);
      if (createdCategory != null)
      {
        return CreatedAtAction(nameof(GetCategory), new
        {
          categoryId = createdCategory.CategoryId
        }, createdCategory);
      }

      return Ok(new SuccessMessage<Category>
      {
        Success = true,
        Message = "Category is created successfully",
        Data = createdCategory
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot create new category");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpPut("{categoryId}")]
  public async Task<IActionResult> UpdateCategory(string categoryId, Category updateCategory)
  {
    try
    {
      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return BadRequest("Invalid category ID Format");
      }
      var category = await _dbContext.UpdateCategoryService(categoryIdGuid, updateCategory);
      if (category == null)
      {
        return NotFound(new ErrorMessage
        {
          Message = "No Category To Founded To Update"
        });
      }
      return Ok(new SuccessMessage<Category>
      {
        Success = true,
        Message = "Category is updated successfully",
        Data = category
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot update the category");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpDelete("{categoryId}")]
  public async Task<IActionResult> DeleteCategory(string categoryId)
  {
    try
    {
      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return BadRequest("Invalid Category ID Format");
      }
      var result = await _dbContext.DeleteCategoryService(categoryIdGuid);
      if (!result)
      {
        return NotFound(new ErrorMessage
        {
          Message = "The category is not found to be deleted"
        });
      }
      return Ok(new { success = true, message = "Category is deleted successfully" });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, the category can not deleted");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }
}
