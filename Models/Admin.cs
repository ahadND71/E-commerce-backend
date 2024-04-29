public class Admin
{
  public Guid AdminId { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
  public string Address { get; set; } = string.Empty;
  public string Image { get; set; } = string.Empty;
  public bool IsBanned { get; set; }
  public DateTime CreatedAt { get; set; }
}