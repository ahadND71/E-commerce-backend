using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class AddressDto
{

    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? AddressLine1 { get; set; }

    [MaxLength(100)]
    public string? AddressLine2 { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(100)]
    public string? Province { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(10)]
    public string? ZipCode { get; set; }

    //? Relations

    public Guid CustomerId { get; set; }
}
