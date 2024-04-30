using Microsoft.AspNetCore.Mvc;
using api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using api.Helpers;

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
  public async Task<IActionResult> GetAllCategories()
  {
    try
    {
      var categories = await _categoryService.GetAllCategoryService();

      if (categories.ToList().Count < 1){
        return NotFound(new ErrorMessage{
          Message = "No Categories To Display"
        });
      }




      return Ok(new SuccessMessage <IEnumerable<Category>>{
         Message = "Categories are returned succeefully" , 
         Data = categories });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not return the category list");
      return StatusCode(500, new ErrorMessage {
        Message = ex.Message
      });
    }
  }
  [HttpGet("{categoryId:guid}")]
  public async Task<IActionResult> GetCategory(string categoryId)
  {
    try
    {

      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return BadRequest("Invalid category ID Format");
      }
      var category = await _categoryService.GetCategoryById(categoryIdGuid);
      if (category == null)
      {
        return NotFound(new ErrorMessage
        {
          Message = $"No Category Found With ID : ({categoryIdGuid})"
        });
      }
      else
      {
        return Ok( new SuccessMessage<Category>{ 
          Success = true, 
          Message = "Category is returned succeefully" , 
          Data = category });
      }

    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not return the category");
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

      var createdCategory = await _categoryService.CreateCategoryService(newCategory);
      if (createdCategory != null ){
      return CreatedAtAction(nameof(GetCategory), new
      {
        categoryId = createdCategory.CategoryId
      }, createdCategory); }

      return Ok(new SuccessMessage<Category> 
      { Success = true, 
        Message = "Category is created succeefully" , 
        Data = createdCategory });
      }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not create new category");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }




  [HttpPut("{categoryId:guid}")]
  public async Task<IActionResult> UpdateCategory(string categoryId, Category updateCategory)
  {
    try
    {
      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return BadRequest("Invalid category ID Format");
      }
      var category = await _categoryService.UpdateCategoryService(categoryIdGuid, updateCategory);
      if (category == null)
      {
        return NotFound(new ErrorMessage
        {
          Message = "No Category To Founed To Update"
        });
      }
      return Ok(new SuccessMessage<Category> { 
        Success = true, 
        Message = "Category is updated succeefully" , 
        Data = category });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not update the category");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }



  [HttpDelete("{categoryId:guid}")]
  public async Task<IActionResult> DeleteCategory(string categoryId)
  {
    try
    {
      if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
      {
        return BadRequest("Invalid Category ID Format");
      }
      var result = await _categoryService.DeleteCategoryService(categoryIdGuid);
      if (!result)
      {
        return NotFound(new ErrorMessage
        {
          Message = "The category is not found to be deleted"
        });
      }
      return Ok(new { success = true, message = "Category is deleted succeefully" });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , the category can not deleted");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }
}
