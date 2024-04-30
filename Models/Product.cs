using System.ComponentModel.DataAnnotations;

public class Product
{
  [Key]
  public Guid ProductId { get; set; }

  // [Required]
  // public Guid CategoryId { get; set; }

  // [Required]
  // public Guid AdminId { get; set; }

  [Required(ErrorMessage = "Product name is requierd")]
  [MaxLength(100), MinLength(30)]
  public string Name { get; set; }


  [MaxLength(100)]
  public string Slug { get; set; } = string.Empty;



  [MaxLength(300)]
  public string Description { get; set; }



  [Required(ErrorMessage = "The price is required")]
  [MaxLength(10), MinLength(1)]
  public double Price { get; set; }



  [Required(ErrorMessage = "SKU is requierd")]
  [MaxLength(100)]
  public string SKU { get; set; }

  [Required(ErrorMessage = "Stock quantity is requierd")]
  [MaxLength(100)]
  public int StockQuantity { get; set; }



  [Required(ErrorMessage = "Product image is requierd")]
  [MaxLength(250)]
  public string ImgUrl { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.Now;
  
  public DateTime UpdatedAt { get; set; } = DateTime.Now;

  //
  public Category Category { get; set; }
  public Admin Admin { get; set; }
  public ICollection<Review> Reviews { get; set; }
  public ICollection<OrderProduct> OrderProducts { get; set; }
}
