using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using Backend.Dtos;

namespace Backend.Services;

public class ProductService
{
    private readonly AppDbContext _dbContext;

    public ProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<Product>> GetAllProductService(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? sortBy,
        string? sortOrder,
        decimal? minPrice,
        decimal? maxPrice
    )
    {
        var query = _dbContext.Products.AsQueryable(); // Start with a queryable
        // Apply filtering
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        // Apply Sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortOrder == "asc")
            {
                query = query.OrderBy(GetSortExpression(sortBy));
            }
            else // 'desc' as default if not provided
            {
                query = query.OrderByDescending(GetSortExpression(sortBy));
            }
        }
        else
        {
            // Default sorting (by name)
            query = query.OrderBy(x => x.Name);
        }

        // Pagination (remains the same)
        return await query
            .Include(r => r.Reviews) // Include relations
            .Include(op => op.OrderProducts)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }


    // Helper method for dynamic sorting
    private Expression<Func<Product, object>> GetSortExpression(string sortBy)
    {
        return sortBy.ToLowerInvariant() switch
        {
            "price" => p => p.Price,
            "createdat" or "date" => p => p.CreatedAt,
            "updatedat" or "latest" => p => p.UpdatedAt,
            "stock" or "stockquantity" => p => p.StockQuantity,
            "category" => p => p.CategoryId,
            _ => p => p.Name, // Default sort field
        };
    }


    // Get a single product by its Id
    public async Task<Product?> GetProductByIdService(Guid productId)
    {
        return await _dbContext.Products
            .Include(r => r.Reviews) // Include relations
            .Include(op => op.OrderProducts)
            .FirstOrDefaultAsync(p => p.ProductId == productId);
    }


    // Search for products by name or description with pagination
    public async Task<IEnumerable<Product>> SearchProductsService(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = _dbContext.Products
            .Where(p => !string.IsNullOrEmpty(searchTerm) &&
                        (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)))
            .Include(r => r.Reviews) // should we include the relations?
            .Include(op => op.OrderProducts)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }


    // Get the count of products matching the search term (helper method for SearchProducts in ProductController.cs)
    public async Task<int> GetProductCountBySearchTerm(string? searchTerm)
    {
        var query = _dbContext.Products.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
        }

        return await query.CountAsync();
    }


    // Helper function to get total product count
    public async Task<int> GetTotalProductCount()
    {
        return await _dbContext.Products.CountAsync();
    }


    // Creates a new product -- with default values like Guid/slug/date
    public async Task<Product> CreateProductService(Product newProduct)
    {
        // Check if a category with the same name already exists
        var existingProduct = await _dbContext.Products
            .FirstOrDefaultAsync(c => c.Name == newProduct.Name);

        if (existingProduct != null)
        {
            throw new InvalidOperationException("Product Is Already Exists");
        }

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
                // Handle invalid CategoryId here
            }
        }
        else
        {
            newProduct.CategoryId = null; // Set CategoryId to null if not provided
        }

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        return newProduct;
    }


    // Update a product
    public async Task<Product?> UpdateProductService(Guid productId, ProductDto updateProduct)
    {
        var existingProduct = await _dbContext.Products.FindAsync(productId);
        if (existingProduct != null)
        {
            existingProduct.Name = updateProduct.Name.IsNullOrEmpty() ? existingProduct.Name : updateProduct.Name;
            existingProduct.Slug = SlugGenerator.GenerateSlug(existingProduct.Name);
            existingProduct.Description = updateProduct.Description.IsNullOrEmpty() ? existingProduct.Description : updateProduct.Description;
            existingProduct.Price = updateProduct.Price ?? existingProduct.Price;
            existingProduct.ImgUrl = updateProduct.ImgUrl.IsNullOrEmpty() ? existingProduct.ImgUrl : updateProduct.ImgUrl;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        return existingProduct;
    }


    // Delete a product
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
    public async Task<Product?> GetProductBySlugService(string slug)
    {
        return await _dbContext.Products
            .Include(r => r.Reviews) // Include relations
            .Include(op => op.OrderProducts)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(Guid categoryId, int pageNumber, int pageSize)
    {
        var query = _dbContext.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(r => r.Reviews)
            .Include(op => op.OrderProducts)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetNewArrivals(int pageNumber, int pageSize)
    {
        var threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);
        var query = _dbContext.Products
            .Where(p => p.CreatedAt >= threeMonthsAgo)
            .Include(r => r.Reviews) // Include relations
            .Include(op => op.OrderProducts)
            .OrderByDescending(p => p.CreatedAt) // Sort by newest first
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }



}