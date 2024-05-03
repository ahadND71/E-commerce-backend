using System.ComponentModel.DataAnnotations;

public class Category
{
  public Guid CategoryId { get; set; }

  [Required(ErrorMessage = "Category name is requierd")]
  [MaxLength(100), MinLength(2)]
  public string Name { get; set; }

  public string Slug { get; set; }

  [MaxLength(300)]
  public string Description { get; set; } = string.Empty;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  // Relationship: Collection navigation containing dependents
  public Guid AdminId { get; set; }
  public ICollection<Product> Products { get; } = new List<Product>();
}

