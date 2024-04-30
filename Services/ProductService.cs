using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

namespace api.Services;

public class ProductService
{
  private readonly AppDbContext _dbContext;

  public ProductService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }


  public async Task<IEnumerable<Product>> GetAllProductService()
  {
    return await _dbContext.Products.ToListAsync();
  }


  public async Task<Product?> GetProductByIdService(Guid productId)
  {
    return await _dbContext.Products.FindAsync(productId);
  }


  public async Task<Product> CreateProductService(Product newProduct)
  {
    newProduct.ProductId = Guid.NewGuid();
    newProduct.Slug = SlugGenerator.GenerateSlug(newProduct.Name);
    newProduct.CreatedAt = DateTime.UtcNow;
    newProduct.UpdatedAt = DateTime.UtcNow;
    _dbContext.Products.Add(newProduct);
    await _dbContext.SaveChangesAsync();
    return newProduct;
  }


  public async Task<Product?> UpdateProductService(Guid productId, Product updateProduct)
  {
    var existingProduct = await _dbContext.Products.FindAsync(productId);
    if (existingProduct != null)
    {
      existingProduct.Name = updateProduct.Name ?? existingProduct.Name;
      existingProduct.Slug = updateProduct.Slug ?? existingProduct.Slug;
      existingProduct.Description = updateProduct.Description ?? existingProduct.Description;
      existingProduct.Price = updateProduct.Price;
      existingProduct.SKU = updateProduct.SKU ?? existingProduct.SKU;
      existingProduct.ImgUrl = updateProduct.ImgUrl ?? existingProduct.ImgUrl;
      existingProduct.UpdatedAt = DateTime.UtcNow;
      await _dbContext.SaveChangesAsync();
    }
    return existingProduct;
  }


  public async Task<bool> DeleteProductService(Guid productId)
  {
    var productToRemove = await _dbContext.Products.FindAsync(productId);
    if (productToRemove != null)
    {
      _dbContext.Products.Remove(productToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}
