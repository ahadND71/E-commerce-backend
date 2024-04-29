public class Address
{
  public Guid AddressId { get; set; }
  public Guid CustomerId { get; set; }
  public required string Name { get; set; }
  public string AddressLine1 { get; set; } = string.Empty;
  public string AddressLine2 { get; set; } = string.Empty;
  public required string Country { get; set; }
  public required string Province { get; set; }
  public required string City { get; set; }
  public string ZipCode { get; set; } = string.Empty;

}