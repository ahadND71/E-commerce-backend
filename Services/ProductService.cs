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


  public async Task<Product?> GetProductByIdService(Guid id)
  {
    return await _dbContext.Products.FindAsync(id);
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


  public async Task<Product?> UpdateProductService(Guid id, Product updateProduct)
  {
    var foundedProduct = await _dbContext.Products.FindAsync(id);
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


  public async Task<bool> DeleteProductService(Guid id)
  {
    var productToRemove = await _dbContext.Products.FindAsync(id);
    if (productToRemove != null)
    {
      _dbContext.Products.Remove(productToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}
