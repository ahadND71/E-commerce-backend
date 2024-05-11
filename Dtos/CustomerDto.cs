using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos;

public class CustomerDto
{
    public Guid CustomerId { get; set; }

    [MaxLength(100), MinLength(2)] public string? FirstName { get; set; }

    [MaxLength(100), MinLength(2)] public string? LastName { get; set; }

    [MaxLength(100), MinLength(6)]
    [EmailAddress(ErrorMessage = "Email address is not valid")]
    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Image { get; set; }

    public bool IsBanned { get; set; }

    public DateTime CreatedAt { get; set; }

    //? Relations
    public ICollection<Address> Addresses { get; } = new List<Address>();
    public ICollection<Order> Orders { get; } = new List<Order>();
    public ICollection<Review> Reviews { get; } = new List<Review>();
}