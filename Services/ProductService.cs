using Microsoft.AspNetCore.Http.HttpResults;

public class ProductService
{
  public static List<Product> _products = new List<Product>(){
    new Product{
        ProductId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        Name ="Dell XPS 13",
        Slug = "dell-xps-13",
        Description ="Ultra-thin laptop with 4K display",
        Price = 1500.00,
        SKU ="dellxps13",
        StockQuantity = 20 ,
        ImgUrl ="https://example.com/dell_xps_13.jpg"
    },
    new Product{
        ProductId=  Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a999929"),
        Name ="iPhone 13 Pro",
        Slug = "iphone-13-pro",
        Description ="Latest iPhone with A15 Bionic chip",
        Price = 1200.00,
        SKU ="iphone13pro",
        StockQuantity = 30 ,
        ImgUrl ="https://example.com/iphone_13_pro.jpg"
    },
    new Product{
        ProductId =  Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3339"),
        Name ="airpods-pro",
        Slug = "earphones",
        Description ="Active Noise Cancellation for immersive sound",
        Price = 250.00,
        SKU ="airpodspro",
        StockQuantity = 40 ,
        ImgUrl ="https://example.com/airpods_pro.jpg"
    }
  };
  // SERVICES
  public IEnumerable<Product> GetAllProductService()
  {
    return _products;
  }
  public Product? GetProductByIdService(Guid id)
  {
    return _products.Find(product => product.ProductId == id);
  }
  public Product CreateProductService(Product newProduct)
  {
    newProduct.ProductId = Guid.NewGuid();
    newProduct.CreatedAt = DateTime.Now;
    newProduct.UpdatedAt = DateTime.Now;


    _products.Add(newProduct);
    return newProduct;
  }
  public Product? UpdateProductService(Guid id, Product updateProduct)
  {

    var foundedProduct = _products.FirstOrDefault(product => product.ProductId == id);
    if (foundedProduct != null)
    {
      foundedProduct.Name = updateProduct.Name;
      foundedProduct.Slug = updateProduct.Slug;
      foundedProduct.Description = updateProduct.Description;
      foundedProduct.Price = updateProduct.Price;
      foundedProduct.SKU = updateProduct.SKU;
      foundedProduct.ImgUrl = updateProduct.ImgUrl;
      foundedProduct.UpdatedAt = DateTime.Now;
    }
    return foundedProduct;
  }
  public bool DeleteProductService(Guid id)
  {
    var ProductToRemove = _products.FirstOrDefault(product => product.ProductId == id);
    if (ProductToRemove != null)
    {
      _products.Remove(ProductToRemove);
      return true;
    }
    return false;
  }
}