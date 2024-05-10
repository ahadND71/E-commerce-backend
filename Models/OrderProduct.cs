using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class OrderProduct
{
    public Guid OrderProductId { get; set; }

    [Required(ErrorMessage = "Number of quantity of the item ordered can not be empty")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "The price of unit can not be empty")]
    public decimal ProductPrice { get; set; }

    //? Relations

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
}