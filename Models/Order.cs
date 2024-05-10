using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Order
{
    public Guid OrderId { get; set; }

    [Required(ErrorMessage = "The total price of the order can not be empty")]
    [Range(0, int.MaxValue, ErrorMessage = "Total Price Must Be Positive Number")]
    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "Pending...";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //? Relations

    public Guid CustomerId { get; set; }
    public Guid AddressId { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}