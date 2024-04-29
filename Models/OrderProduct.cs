public class OrderProduct
{
  public Guid OrderItemId { get; set; }
  public required int Quantity { get; set; }
  public required int ProductPrice { get; set; }
}