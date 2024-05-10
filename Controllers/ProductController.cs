using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;

namespace Backend.Controllers;


[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
  public ProductService _productService;
  public ProductController(ProductService productService)
  {
    _productService = productService;
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

    var products = await _productService.GetAllProductService(pageNumber, pageSize, searchTerm, sortBy, sortOrder, minPrice, maxPrice);
    int totalProductCount = await _productService.GetTotalProductCount();

    return ApiResponse.Success
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


  [AllowAnonymous]
  [HttpGet("{productId:guid}")]
  public async Task<IActionResult> GetProduct(Guid productId)
  {
    var product = await _productService.GetProductByIdService(productId);
    if (product == null)
    {
      return ApiResponse.NotFound(
               $"No Product Found With ID : ({productId})");
    }
    else
    {
      return ApiResponse.Success(
        product,
        "Product is returned successfully"
      );
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
    var products = await _productService.SearchProductsService(pageNumber, pageSize, searchTerm);

    int totalProductCount = await _productService.GetProductCountBySearchTerm(searchTerm);

    return ApiResponse.Success
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


  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<IActionResult> CreateProduct(Product newProduct)
  {
    var createdProduct = await _productService.CreateProductService(newProduct);
    if (createdProduct != null)
    {
      return ApiResponse.Created(createdProduct, "Product is created successfully");
    }
    else
    {
      return ApiResponse.ServerError("Error when creating new product");
    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{productId}")]
  public async Task<IActionResult> UpdateProduct(string productId, ProductDto updateProduct)
  {
    if (!Guid.TryParse(productId, out Guid productIdGuid))
    {
      return ApiResponse.BadRequest("Invalid product ID Format");
    }

    var product = await _productService.UpdateProductService(productIdGuid, updateProduct);
    if (product == null)
    {
      return ApiResponse.NotFound("No Product Founded To Update");
    }
    return ApiResponse.Success<Product>(
        product,
        "Product Is Updated Successfully"
    );
  }


  [Authorize(Roles = "Admin")]
  [HttpDelete("{productId}")]
  public async Task<IActionResult> DeleteProduct(string productId)
  {
    if (!Guid.TryParse(productId, out Guid productIdGuid))
    {
      return ApiResponse.BadRequest("Invalid product ID Format");
    }

    var result = await _productService.DeleteProductService(productIdGuid);
    if (!result)
    {
      return ApiResponse.NotFound("The Product is not found to be deleted");
    }
    return ApiResponse.Success(" Product is deleted successfully");
  }
}
