using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderProduct
{
  [Key]
  public Guid OrderItemId { get; set; }

  [Required]
  public Guid OrderId { get; set; }

  [Required]
  public Guid ProductId { get; set; }

  [Required]
  public int Quantity { get; set; }

  [Required]
  public int ProductPrice { get; set; }

  //
  public Order Order { get; set; }
  public Product Product { get; set; }
}
