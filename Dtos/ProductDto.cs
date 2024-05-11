using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class ProductDto
{
    [MaxLength(100), MinLength(2)] public string? Name { get; set; }

    [MaxLength(300)] public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Price must be at least 1")]
    public decimal? Price { get; set; }

    [MaxLength(100)] public string? SKU { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity Must Be Positive Number")]
    public int? StockQuantity { get; set; }

    [MaxLength(250)] public string? ImgUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}