using System.ComponentModel.DataAnnotations;

public class Category
{
  [Key]
  public Guid CategoryId { get; set; }

  [Required]
  public string Name { get; set; }

  public string Description { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }

}
