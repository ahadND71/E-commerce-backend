using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(100), MinLength(2)]
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    [MaxLength(300)] public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //? Relations

    public Guid? AdminId { get; set; }
    public ICollection<Product> Products { get; } = new List<Product>();
}