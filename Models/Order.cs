public class Order
{
  public Guid OrderId { get; set; }
  public required int TotalAmount { get; set; }
  public string Status { get; set; } = "Pending...";
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public DateTime UpdatedAt { get; set; } = DateTime.Now;
}