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
    return await _dbContext.Products
    .Include(r => r.Reviews)
    .Include(op => op.OrderProducts)
    .ToListAsync();
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

    if (newProduct.CategoryId != Guid.Empty)
    {
      var category = await _dbContext.Categories.FindAsync(newProduct.CategoryId);
      if (category != null)
      {
        newProduct.CategoryId = category.CategoryId;
      }
      else
      {
        // Handle invalid CategoryId here if needed
      }
    }
    else
    {
      newProduct.CategoryId = null; // Set CategoryId to null if not provided
    }

    _dbContext.Products.Add(newProduct);
    await _dbContext.SaveChangesAsync();

    Console.WriteLine($"ProductId of the new product: {newProduct.ProductId}"); // Debug log
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
