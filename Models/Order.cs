using System.ComponentModel.DataAnnotations;

public class Order
{
  [Key]
  public Guid OrderId { get; set; }



  [Required(ErrorMessage = "The total amount of the order can not be Empty")]
  public int TotalAmount { get; set; }


  public string Status { get; set; } = "Pending...";

  
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public DateTime UpdatedAt { get; set; } = DateTime.Now;

  //
  public Guid CustomerId { get; set; }
  public Guid AddressId { get; set; }
  public Customer Customer { get; set; }
  public ICollection<OrderProduct> OrderProducts { get; set; }

}
