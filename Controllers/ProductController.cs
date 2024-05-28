using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;
using SendGrid.Helpers.Errors.Model;


namespace Backend.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(
        int pageNumber = 1,
        int pageSize = 100,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortOrder = null,
        decimal? minPrice = null,
        decimal? maxPrice = null
    )
    {
        var products = await _productService.GetAllProductService(pageNumber, pageSize, searchTerm, sortBy, sortOrder, minPrice, maxPrice);
        int totalCount = await _productService.GetTotalProductCount();
        if (totalCount < 1)
        {
            throw new NotFoundException("No Products To Display");
        }
        return ApiResponse.Success
        (
            products,
            "Products are returned successfully",
            new PaginationMeta
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            }
        );
    }


    [AllowAnonymous]
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var product = await _productService.GetProductByIdService(productId) ?? throw new NotFoundException($"No Product Found With ID : ({productId})");

        return ApiResponse.Success(
            product,
            "Product is returned successfully"
        );
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(
        int pageNumber = 1,
        int pageSize = 100,
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



    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product newProduct)
    {
        var createdProduct = await _productService.CreateProductService(newProduct) ?? throw new Exception("Error when creating new product");

        return ApiResponse.Created(createdProduct, "Product is created successfully");
    }


    // [Authorize(Roles = "Admin")]
    [AllowAnonymous]

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


    // [Authorize(Roles = "Admin")]
    [AllowAnonymous]

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

    [AllowAnonymous]
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug(string slug)
    {
        var product = await _productService.GetProductBySlugService(slug) ?? throw new NotFoundException($"No Product Found With Slug: {slug}");

        return ApiResponse.Success(
            product,
            "Product is returned successfully"
        );
    }

    [AllowAnonymous]
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(Guid categoryId)
    {
        try
        {
            var products = await _productService.GetProductsByCategory(categoryId, 1, 100); // Adjust pagination parameters as needed
            if (products == null || !products.Any())
            {
                return NotFound("No products found for the given category.");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [AllowAnonymous]
    [HttpGet("new-arrivals")]
    public async Task<IActionResult> GetNewArrivals(int pageNumber = 1, int pageSize = 10)
    {
        var products = await _productService.GetNewArrivals(pageNumber, pageSize);
        if (!products.Any())
        {
            return NotFound("No new arrivals found.");
        }
        return Ok(products);
    }

}



