using System.ComponentModel.DataAnnotations;

public class Product
{
  public Guid ProductId { get; set; }

  // [Required]
  // public Guid CategoryId { get; set; }

  // [Required]
  // public Guid AdminId { get; set; }

  [Required(ErrorMessage = "Product name is requierd")]
  [MaxLength(100), MinLength(2)]
  public string Name { get; set; }

  public string Slug { get; set; }

  [MaxLength(300)]
  public string Description { get; set; }

  [Required(ErrorMessage = "The price is required")]
  [Range(1, int.MaxValue, ErrorMessage = "Price must be at least 1")]
  public decimal Price { get; set; }

  [Required(ErrorMessage = "SKU is requierd")]
  [MaxLength(100)]
  public string SKU { get; set; }

  [Required(ErrorMessage = "Stock quantity is requierd")]
  [Range(0, int.MaxValue, ErrorMessage = "Error StockQuantity")]
  public int StockQuantity { get; set; }

  [Required(ErrorMessage = "Product image is requierd")]
  [MaxLength(250)]
  public string ImgUrl { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  //
  // public Category Category { get; set; }
  // public Admin Admin { get; set; }
  // public ICollection<Review> Reviews { get; set; }
  // public ICollection<OrderProduct> OrderProducts { get; set; }
}
