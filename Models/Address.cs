using System.ComponentModel.DataAnnotations;

public class Address
{
  public Guid AddressId { get; set; }

  // public Guid CustomerId { get; set; }

  [Required(ErrorMessage = "Address name is required")]
  [MaxLength(100)]
  public string Name { get; set; }

  [Required(ErrorMessage = "At least one address line should be added")]
  [MaxLength(100)]
  public string AddressLine1 { get; set; }

  [MaxLength(100)]
  public string AddressLine2 { get; set; } = string.Empty;

  [Required(ErrorMessage = "Country name is required")]
  [MaxLength(100)]
  public string Country { get; set; }

  [Required(ErrorMessage = "Province name is required")]
  [MaxLength(100)]
  public string Province { get; set; }

  [Required(ErrorMessage = "City name is required")]
  [MaxLength(100)]
  public string City { get; set; }

  [MaxLength(10)]
  public string ZipCode { get; set; } = string.Empty;
}
