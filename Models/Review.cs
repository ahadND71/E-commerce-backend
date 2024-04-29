public class Review
{
  public Guid ReviewId { get; set; }
  public Guid ProductId { get; set; }
  public Guid CustomerId { get; set; }
  public Guid OrderId { get; set; }
  public required int Rating { get; set; }
  public string Comment { get; set; } = string.Empty;
  public DateTime ReviewDate { get; set; }
  public string Status { get; set; } = "Pending";
  public bool IsAnonymous { get; set; } = false;

}