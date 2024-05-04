using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


[Index(nameof(Email), IsUnique = true)]
public class Customer
{
  public Guid CustomerId { get; set; }

  [Required(ErrorMessage = "First name is requierd")]
  [MaxLength(100), MinLength(2)]
  public string FirstName { get; set; }

  [Required(ErrorMessage = "Last name is requierd")]
  [MaxLength(100), MinLength(2)]
  public string LastName { get; set; }

  [Required(ErrorMessage = "Email is requierd")]
  [MaxLength(100), MinLength(6)]
  [EmailAddress(ErrorMessage = "Email address is not valid")]
  public string Email { get; set; }

  [Required(ErrorMessage = "Password is requierd")]
  [MaxLength(25), MinLength(8)]
  public string Password { get; set; }

  public string Image { get; set; } = string.Empty;

  public bool IsBanned { get; set; } = false;

  public DateTime CreatedAt { get; set; }

  //
  // public string AddressId { get; set; } = string.Empty;
  public ICollection<Address> Addresses { get; } = new List<Address>();
  public ICollection<Order> Orders { get; } = new List<Order>();
  public ICollection<Review> Reviews { get; } = new List<Review>();
}
