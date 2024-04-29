using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [ApiController]
  [Route("/api/products")]
  public class ProductController : ControllerBase
  {
    public ProductService _productService;
    public ProductController()
    {
      _productService = new ProductService();
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
      var products = _productService.GetAllProductService();
      return Ok(products);
    }

    [HttpGet("{ProductId}")]
    public IActionResult GetOneProduct(string id)
    {
      if (!Guid.TryParse(id, out Guid ProductId_Guid))
      {
        return BadRequest("Invalid product ID Format");
      }
      var product = _productService.GetProductByIdService(ProductId_Guid);
      return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct(Product newProduct)
    {
      var createdProduct = _productService.CreateProductService(newProduct);
      return CreatedAtAction(nameof(GetOneProduct), new { id = createdProduct.ProductId }, createdProduct);
    }

    [HttpPut("{ProductId}")]
    public IActionResult UpdateProduct(string id, Product updateProduct)
    {
      if (!Guid.TryParse(id, out Guid ProductId_Guid))
      {
        return BadRequest("Invalid product ID Format");
      }
      var product = _productService.UpdateProductService(ProductId_Guid, updateProduct);
      if (product == null)
      {
        return NotFound();
      }
      return Ok(product);
    }

    [HttpDelete("{ProductId}")]
    public IActionResult DeleteProduct(string id)
    {
      if (!Guid.TryParse(id, out Guid ProductId_Guid))
      {
        return BadRequest("Invalid product ID Format");
      }
      var result = _productService.DeleteProductService(ProductId_Guid);
      if (!result)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}