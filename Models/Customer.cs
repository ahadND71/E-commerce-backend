using System.ComponentModel.DataAnnotations;

public class Customer
{
  [Key]
  public Guid CustomerId { get; set; }

  [Required]
  public string FirstName { get; set; }

  [Required]
  public string LastName { get; set; }

  [Required]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }

  public string Address { get; set; } = string.Empty;
  public string Image { get; set; } = string.Empty;
  public bool IsBanned { get; set; }
  public DateTime CreatedAt { get; set; }

  //
  public ICollection<Address> Addresses { get; set; }
  public ICollection<Order> Orders { get; set; }
  public ICollection<Review> Reviews { get; set; }
}
