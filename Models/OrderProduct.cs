using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderProduct
{
  [Key]
  public Guid OrderItemId { get; set; }

  [Key]
  [Column(Order = 1)]
  public Guid OrderId { get; set; }

  [Key]
  [Column(Order = 2)]
  public Guid ProductId { get; set; }

  [Required]
  public int Quantity { get; set; }
  [Required]
  public int ProductPrice { get; set; }

  //
  public Order Order { get; set; }
  public Product Product { get; set; }
}
