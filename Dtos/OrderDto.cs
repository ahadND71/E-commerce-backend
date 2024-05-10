using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos;

public class OrderDto
{
    [Range(0, int.MaxValue, ErrorMessage = "Total Price Must Be Positive Number")]
    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    //? Relations

    public Guid CustomerId { get; set; }
    public Guid AddressId { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
