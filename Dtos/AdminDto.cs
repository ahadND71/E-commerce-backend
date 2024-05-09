public class AdminDto
{
    public Guid AdminId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Mobile { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
