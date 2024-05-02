using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderProduct
{
  [Key]
  public Guid OrderItemId { get; set; }

  [Required(ErrorMessage = "Number of quantity of the item ordered can not be Empty")]
  // [MaxLength(10), MinLength(1)]
  public int? Quantity { get; set; }

  [Required(ErrorMessage = "The price of unit can not be Empty")]
  public int ProductPrice { get; set; }

  // Relations
  public Guid OrderId { get; set; }
  public Guid ProductId { get; set; }
}
