using Microsoft.AspNetCore.Authorization;
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


  [AllowAnonymous]
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

      return ApiResponse.Success<IEnumerable<Product>>
      (
        products,
         "Products are returned successfully",
        new PaginationMeta
        {
          CurrentPage = pageNumber,
          PageSize = pageSize,
          TotalCount = totalProductCount
        }
      );
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Product list");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [AllowAnonymous]
  [HttpGet("{productId:guid}")]
  public async Task<IActionResult> GetProduct(Guid productId)
  {
    try
    {
      var product = await _dbContext.GetProductByIdService(productId);
      if (product == null)
      {
        return ApiResponse.NotFound(
                 $"No Product Found With ID : ({productId})");
      }
      else
      {
        return ApiResponse.Success<Product>(
          product,
          "Product is returned successfully"
        );
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Product");
      return ApiResponse.ServerError(ex.Message);

    }
  }

  [AllowAnonymous]
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

      return ApiResponse.Success<IEnumerable<Product>>
      (
        products,
        "Products are returned successfully",
        new PaginationMeta
        {
          CurrentPage = pageNumber,
          PageSize = pageSize,
          TotalCount = totalProductCount
        }
      );
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, can't search for products");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<IActionResult> CreateProduct(Product newProduct)
  {
    try
    {
      var createdProduct = await _dbContext.CreateProductService(newProduct);
      if (createdProduct != null)
      {
        return ApiResponse.Created<Product>(createdProduct, "Product is created successfully");
      }
      else
      {
        return ApiResponse.ServerError("Error when creating new product");

      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot create new Product");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{productId}")]
  public async Task<IActionResult> UpdateProduct(string productId, Product updateProduct)
  {
    try
    {
      if (!Guid.TryParse(productId, out Guid productIdGuid))
      {
        return ApiResponse.BadRequest("Invalid product ID Format");
      }
      var product = await _dbContext.UpdateProductService(productIdGuid, updateProduct);
      if (product == null)
      {
        return ApiResponse.NotFound("No Product Founded To Update");
      }
      return ApiResponse.Success<Product>(
          product,
          "Product Is Updated Successfully"
      );
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred , can not update the Product ");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpDelete("{productId}")]
  public async Task<IActionResult> DeleteProduct(string productId)
  {
    try
    {
      if (!Guid.TryParse(productId, out Guid productIdGuid))
      {
        return ApiResponse.BadRequest("Invalid product ID Format");
      }
      var result = await _dbContext.DeleteProductService(productIdGuid);
      if (!result)
      {
        return ApiResponse.NotFound("The Product is not found to be deleted");
      }
      return ApiResponse.Success(" Product is deleted successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, the Product can not deleted");
      return ApiResponse.ServerError(ex.Message);

    }
  }
}
