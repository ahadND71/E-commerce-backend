using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class OrderProductDto
{

    [Range(1, int.MaxValue, ErrorMessage = "Quantity Must Be Greater Than Zero")]
    public int? Quantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Product Price Must Be Greater Than Zero")]
    public decimal? ProductPrice { get; set; }
}
