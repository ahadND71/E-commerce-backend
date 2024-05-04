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
  public async Task<IActionResult> GetAllProducts(
     int pageNumber = 1,
     int pageSize = 10,
     string? searchTerm = null,
     string? sortBy = null,
     string? sortOrder = null,
     decimal? minPrice = null,
     decimal? maxPrice = null
  )
  {
    try
    {
      var products = await _dbContext.GetAllProductService(pageNumber, pageSize, searchTerm, sortBy, sortOrder, minPrice, maxPrice);

      int totalProductCount = await _dbContext.GetTotalProductCount();

      return Ok(new SuccessMessage<IEnumerable<Product>>
      {
        Message = "Products are returned successfully",
        Data = products,
        Meta = new PaginationMeta
        {
          CurrentPage = pageNumber,
          PageSize = pageSize,
          TotalCount = totalProductCount
        }
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, can not return the Product list");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpGet("{productId:guid}")]
  public async Task<IActionResult> GetProduct(Guid productId)
  {
    try
    {
      var product = await _dbContext.GetProductByIdService(productId);
      if (product == null)
      {
        return NotFound(new ErrorMessage
        {
          Message = $"No Product Found With ID : ({productId})"
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


  [HttpGet("search")]
  public async Task<IActionResult> SearchProducts(
    int pageNumber = 1,
    int pageSize = 10,
    string? searchTerm = null
  )
  {
    try
    {
      var products = await _dbContext.SearchProductsService(pageNumber, pageSize, searchTerm);

      int totalProductCount = await _dbContext.GetProductCountBySearchTerm(searchTerm);

      return Ok(new SuccessMessage<IEnumerable<Product>>
      {
        Message = "Products are returned successfully",
        Data = products,
        Meta = new PaginationMeta
        {
          CurrentPage = pageNumber,
          PageSize = pageSize,
          TotalCount = totalProductCount
        }
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, can't search for products");
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
        return CreatedAtAction(nameof(GetProduct), new { productId = createdProduct.ProductId }, createdProduct);
      }
      return Ok(new SuccessMessage<Product>
      {
        Message = "Product is created succeefully",
        Data = createdProduct
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured, can not create new Product");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpPut("{productId}")]
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
      Console.WriteLine($"An error occured, can not update the Product ");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpDelete("{productId}")]
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
