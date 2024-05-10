using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Address
{
    public Guid AddressId { get; set; }

    [Required(ErrorMessage = "Address name is required")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "At least one address line should be added")]
    [MaxLength(100)]
    public string AddressLine1 { get; set; } = string.Empty;

    [MaxLength(100)]
    public string AddressLine2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country name is required")]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Province name is required")]
    [MaxLength(100)]
    public string Province { get; set; } = string.Empty;

    [Required(ErrorMessage = "City name is required")]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [MaxLength(10)]
    public string ZipCode { get; set; } = string.Empty;

    //? Relations

    public Guid CustomerId { get; set; }
}