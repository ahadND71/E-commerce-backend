using Microsoft.AspNetCore.Mvc;
using api.Services;

namespace api.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
  public ProductService _productService;
  public ProductController(ProductService productService)
  {
    _productService = productService;
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
  public async Task<IActionResult> CreateProduct(Product newProduct)
  {
    var createdProduct = await _productService.CreateProductService(newProduct);
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
  public async Task<IActionResult> DeleteProduct(string id)
  {
    if (!Guid.TryParse(id, out Guid ProductId_Guid))
    {
      return BadRequest("Invalid product ID Format");
    }
    var result = await _productService.DeleteProductService(ProductId_Guid);
    if (!result)
    {
      return NotFound();
    }
    return NoContent();
  }
}
