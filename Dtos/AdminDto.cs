using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class AdminDto
{
    public Guid AdminId { get; set; }

    [MaxLength(100), MinLength(2)] public string? FirstName { get; set; }

    [MaxLength(100), MinLength(2)] public string? LastName { get; set; }

    [MaxLength(100), MinLength(6)]
    [EmailAddress(ErrorMessage = "Email address is not valid")]
    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; }
}