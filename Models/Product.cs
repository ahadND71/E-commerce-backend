using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Index(nameof(Name), IsUnique = true)]
public class Product
{
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [MaxLength(100), MinLength(2)]
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    [MaxLength(300)] public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "The price is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be at least 1")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "SKU is required")]
    [MaxLength(100)]
    public string SKU { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity Must Be Positive Number")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "Product image is required")]
    [MaxLength(250)]
    public string ImgUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //? Relations

    public Guid? CategoryId { get; set; }
    public Guid? AdminId { get; set; }

    public ICollection<Review> Reviews { get; } = new List<Review>();
    public ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}