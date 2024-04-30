using System.ComponentModel.DataAnnotations;

public class Customer
{
  [Key]
  public Guid CustomerId { get; set; }

  [Required(ErrorMessage = "First name is requierd")]
  [MaxLength(100), MinLength(30)]
  public string FirstName { get; set; }



  [Required(ErrorMessage = "Last name is requierd")]
  [MaxLength(100), MinLength(30)]
  public string LastName { get; set; }



  [Required(ErrorMessage = "Emailis requierd")]
  [MaxLength(100), MinLength(11)]
  [EmailAddress(ErrorMessage = "Email address is not valid")]
  public string Email { get; set; }



  [Required(ErrorMessage = "Password is requierd")]
  [MaxLength(25), MinLength(8)]
  public string Password { get; set; }


//Why ??
  public string Address { get; set; } = string.Empty;



  public string Image { get; set; } = string.Empty;


  public bool IsBanned { get; set; } = false;


  
  public DateTime CreatedAt { get; set; }

  //
  public ICollection<Address> Addresses { get; set; }
  public ICollection<Order> Orders { get; set; }
  public ICollection<Review> Reviews { get; set; }
}
