using System.ComponentModel.DataAnnotations;

public class Address
{
  [Key]
  public Guid AddressId { get; set; }

  [Required]
  public Guid CustomerId { get; set; }

  [Required]
  public string Name { get; set; }

  public string AddressLine1 { get; set; } = string.Empty;
  public string AddressLine2 { get; set; } = string.Empty;

  [Required]
  public string Country { get; set; }

  [Required]
  public string Province { get; set; }

  [Required]
  public string City { get; set; }

  public string ZipCode { get; set; } = string.Empty;

}
