using System.ComponentModel.DataAnnotations;

public class Review
{
  public Guid ReviewId { get; set; }

  [Required(ErrorMessage = "Product rate is required")]
  [Range(1, 5)]
  public int Rating { get; set; }

  public string Comment { get; set; } = string.Empty;

  public DateTime ReviewDate { get; set; }

  public string Status { get; set; } = "Pending";

  public bool IsAnonymous { get; set; } = false;

  // Relations
  public Guid ProductId { get; set; }
  public Guid CustomerId { get; set; }
  public Guid? OrderId { get; set; }

}
