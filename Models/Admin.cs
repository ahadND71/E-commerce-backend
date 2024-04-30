using System.ComponentModel.DataAnnotations;

public class Admin
{
  [Key]
  public Guid AdminId { get; set; }

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



  [Required(ErrorMessage = "Mobile number is requierd")]
  [MaxLength(13), MinLength(9)]
  public int Mobile { get; set; }



  public string Image { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }

  //
  public ICollection<Product> Products { get; set; }
}
