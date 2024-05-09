public class CustomerDto
{
    public Guid CustomerId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Mobile { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public bool IsBanned { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    internal object Include(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}
