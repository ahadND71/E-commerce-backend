using System.ComponentModel.DataAnnotations;

public class Order
{
  public Guid OrderId { get; set; }

  [Required(ErrorMessage = "The total amount of the order can not be empty")]
  public int TotalAmount { get; set; }

  public string Status { get; set; } = "Pending...";

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  // Relations
  public Guid CustomerId { get; set; }
  public Guid AddressId { get; set; }
  public ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

}
