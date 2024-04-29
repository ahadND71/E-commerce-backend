using System.ComponentModel.DataAnnotations;

public class Admin
{
  [Key]
  public Guid AdminId { get; set; }

  [Required]
  public string FirstName { get; set; }

  [Required]
  public string LastName { get; set; }

  [Required]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }

  [Required]
  public int Mobile { get; set; }

  public string Image { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }

  //
  public ICollection<Product> Products { get; set; }
}
