using Microsoft.EntityFrameworkCore;
using api.Data;

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
    newProduct.CreatedAt = DateTime.Now;
    newProduct.UpdatedAt = DateTime.Now;
    _dbContext.Products.Add(newProduct);
    await _dbContext.SaveChangesAsync();
    return newProduct;
  }


  public async Task<Product?> UpdateProductService(Guid productId, Product updateProduct)
  {
    var foundedProduct = await _dbContext.Products.FindAsync(productId);
    if (foundedProduct != null)
    {
      foundedProduct.Name = updateProduct.Name;
      foundedProduct.Slug = updateProduct.Slug;
      foundedProduct.Description = updateProduct.Description;
      foundedProduct.Price = updateProduct.Price;
      foundedProduct.SKU = updateProduct.SKU;
      foundedProduct.ImgUrl = updateProduct.ImgUrl;
      foundedProduct.UpdatedAt = DateTime.Now;
      await _dbContext.SaveChangesAsync();
    }
    return foundedProduct;
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
