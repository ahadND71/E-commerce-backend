using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Helpers;

namespace api.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
  public ProductService _dbContext;
  public ProductController(ProductService productService)
  {
    _dbContext = productService;
  }


  [HttpGet]
  public async Task<IActionResult> GetAllProducts()
  {
    try
    {

      var products = await _dbContext.GetAllProductService();
      if (products.ToList().Count < 1)
      {
        return NotFound(new ErrorMessage
        {
          Message = "No Products To Display"
        });
      }
      return Ok(new SuccessMessage<IEnumerable<Product>>
      {
        Message = "Products are returned succeefully",
        Data = products
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not return the Product list");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpGet("{productId:guid}")]
  public async Task<IActionResult> GetProduct(string productId)
  {
    try
    {
      if (!Guid.TryParse(productId, out Guid productIdGuid))
      {
        return BadRequest("Invalid product ID Format");
      }
      var product = await _dbContext.GetProductByIdService(productIdGuid);
      if (product == null)

      {
        return NotFound(new ErrorMessage
        {
          Message = $"No Product Found With ID : ({productIdGuid})"
        });
      }
      else
      {
        return Ok(new SuccessMessage<Product>
        {
          Success = true,
          Message = "Product is returned succeefully",
          Data = product
        });
      }

    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not return the Product");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }

  }


  [HttpPost]
  public async Task<IActionResult> CreateProduct(Product newProduct)
  {
    try
    {

      var createdProduct = await _dbContext.CreateProductService(newProduct);
      if (createdProduct != null)
      {
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
      }
      return Ok(new SuccessMessage<Product>
      {
        Message = "Product is created succeefully",
        Data = createdProduct
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not create new Product");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpPut("{productId:guid}")]
  public async Task<IActionResult> UpdateProduct(string productId, Product updateProduct)
  {
    try
    {
      if (!Guid.TryParse(productId, out Guid productIdGuid))
      {
        return BadRequest("Invalid product ID Format");
      }
      var product = await _dbContext.UpdateProductService(productIdGuid, updateProduct);
      if (product == null)

      {
        return NotFound(new ErrorMessage
        {
          Message = "No Product To Founed To Update"
        });
      }
      return Ok(new SuccessMessage<Product>
      {
        Message = "Product Is Updated Succeefully",
        Data = product
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not update the Product ");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpDelete("{productId:guid}")]
  public async Task<IActionResult> DeleteProduct(string productId)
  {
    try
    {

      if (!Guid.TryParse(productId, out Guid productIdGuid))
      {
        return BadRequest("Invalid product ID Format");
      }
      var result = await _dbContext.DeleteProductService(productIdGuid);
      if (!result)


      {
        return NotFound(new ErrorMessage
        {
          Message = "The Product is not found to be deleted"
        });
      }
      return Ok(new { success = true, message = " Product is deleted succeefully" });
    }

    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , the Product can not deleted");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }
}
