using System.ComponentModel.DataAnnotations;

public class Review
{
  [Key]
  public Guid ReviewId { get; set; }

  [Required]
  public Guid ProductId { get; set; }

  [Required]
  public Guid CustomerId { get; set; }

  [Required]
  public Guid OrderId { get; set; }

  [Required]
  public int Rating { get; set; }

  public string Comment { get; set; } = string.Empty;

  public DateTime ReviewDate { get; set; }

  public string Status { get; set; } = "Pending";

  public bool IsAnonymous { get; set; } = false;


  //
  public Product Product { get; set; }
  public Customer Customer { get; set; }
  public Order Order { get; set; }
}
