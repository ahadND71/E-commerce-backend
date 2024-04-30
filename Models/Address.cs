using System.ComponentModel.DataAnnotations;

public class Address
{
  [Key]
  public Guid AddressId { get; set; }



  // public Guid CustomerId { get; set; }




  [Required(ErrorMessage = "Address name is requierd")]
  [MaxLength(100) , MinLength(30)]
  public string  Name { get; set; }


  [Required(ErrorMessage = "Atleast one address line should be added")]
  [MaxLength(100), MinLength(30)]
  public string AddressLine1 { get; set; } 



  [MaxLength(100), MinLength(30)]
  public string AddressLine2 { get; set; } = string.Empty;



  [Required(ErrorMessage = "Country name is requierd")]
  [MaxLength(100), MinLength(30)]
  public string Country { get; set; }



  [Required(ErrorMessage = "Province name is requierd")]
  [MaxLength(100), MinLength(30)]
  public string Province { get; set; }



  [Required(ErrorMessage = "City name is requierd")]
  [MaxLength(100), MinLength(30)]
  public string City { get; set; }


  [MaxLength(7), MinLength(5)]
  public string ZipCode { get; set; } = string.Empty;
}
