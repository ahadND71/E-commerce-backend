using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;
using SendGrid.Helpers.Errors.Model;

using System.ComponentModel.DataAnnotations;


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
    var product = await _productService.GetProductByIdService(productId)?? throw new NotFoundException($"No Product Found With ID : ({productId})");

    return ApiResponse.Success(
      product,
      "Product is returned successfully"
    );
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

    if (totalProductCount == 0)
    {
      throw new NotFoundException("Product Not Found In The Store");
    }

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
    var createdProduct = await _productService.CreateProductService(newProduct)?? throw new Exception("Error when creating new product");

    return ApiResponse.Created(createdProduct, "Product is created successfully");
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{productId}")]
  public async Task<IActionResult> UpdateProduct(string productId, ProductDto updateProduct)
  {
    if (!Guid.TryParse(productId, out Guid productIdGuid))
    {
      throw new ValidationException("Invalid product ID Format");
    }

    var product = await _productService.UpdateProductService(productIdGuid, updateProduct) ?? throw new NotFoundException("No Product Founded To Update");

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
      throw new ValidationException("Invalid product ID Format");
    }

    var result = await _productService.DeleteProductService(productIdGuid);
    if (!result)
    {
      throw new NotFoundException("The Product is not found to be deleted");
    }
    return ApiResponse.Success(" Product is deleted successfully");
  }
}
