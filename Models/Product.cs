public class Product
{
  public Guid ProductId { get; set; }
  public required string Name { get; set; }
  public string Slug { get; set; } = string.Empty;
  public required string Description { get; set; }
  public required double Price { get; set; }
  public required string SKU { get; set; }
  public required int StockQuantity { get; set; }
  public required string ImgUrl { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public DateTime UpdatedAt { get; set; } = DateTime.Now;

}
