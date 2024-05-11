using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class ReviewDto
{
    [Range(1, 5, ErrorMessage = "Rate Must Be Between 1 and 5")]
    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; }

    public string? Status { get; set; }

    public bool IsAnonymous { get; set; }
}