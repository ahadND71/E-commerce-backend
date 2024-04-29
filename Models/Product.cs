using System.ComponentModel.DataAnnotations;

public class Product
{
  [Key]
  public Guid ProductId { get; set; }

  [Required]
  public Guid CategoryId { get; set; }

  [Required]
  public Guid AdminId { get; set; }

  [Required]
  public string Name { get; set; }

  public string Slug { get; set; } = string.Empty;

  [Required]
  public string Description { get; set; }

  [Required]
  public double Price { get; set; }

  [Required]
  public string SKU { get; set; }

  [Required]
  public int StockQuantity { get; set; }

  [Required]
  public string ImgUrl { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public DateTime UpdatedAt { get; set; } = DateTime.Now;

  //
  public Category Category { get; set; }
  public Admin Admin { get; set; }
  public ICollection<Review> Reviews { get; set; }
  public ICollection<OrderProduct> OrderProducts { get; set; }
}
