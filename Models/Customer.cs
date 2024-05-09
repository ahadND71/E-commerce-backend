using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


[Index(nameof(Email), IsUnique = true)]
public class Customer
{
  public Guid CustomerId { get; set; }

  [Required(ErrorMessage = "First name is required")]
  [MaxLength(100), MinLength(2)]
  public string FirstName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Last name is required")]
  [MaxLength(100), MinLength(2)]
  public string LastName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Email is required")]
  [MaxLength(100), MinLength(6)]
  [EmailAddress(ErrorMessage = "Email address is not valid")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Mobile number is required")]
  public string Mobile { get; set; } = string.Empty;


  [Required(ErrorMessage = "Password is required")]
  // [MaxLength(25), MinLength(8)]
  public string Password { get; set; } = string.Empty;

  public string Image { get; set; } = string.Empty;

  public bool IsBanned { get; set; } = false;

  public DateTime CreatedAt { get; set; }
  public Guid? ResetToken { get; set; }
  public DateTime? ResetTokenExpiration { get; set; }

  // Relations
  // public string AddressId { get; set; } = string.Empty;
  public ICollection<Address> Addresses { get; } = new List<Address>();
  public ICollection<Order> Orders { get; } = new List<Order>();
  public ICollection<Review> Reviews { get; } = new List<Review>();
}
