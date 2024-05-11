using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class OrderProduct
{
    public Guid OrderProductId { get; set; }

    [Required(ErrorMessage = "Number of quantity of the item ordered can not be empty")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity Must Be Greater Than Zero")]

    public int Quantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Product Price Must Be Greater Than Zero")]
    public decimal ProductPrice { get; set; }

    //? Relations

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
}