using Microsoft.AspNetCore.Mvc;
using api.Services;

namespace api.Controllers;

[ApiController]
[Route("/api/OrderProducts")]
public class OrderProductController : ControllerBase
{
  public OrderProductService _orderProductService;
  public OrderProductController(OrderProductService orderProductService)
  {
    _orderProductService = orderProductService;
  }

  [HttpGet]
  public IActionResult GetAllOrderProducts()
  {
    var OrderProducts = _orderProductService.GetAllOrderProductservice();
    return Ok(OrderProducts);
  }

  [HttpGet("{OrderItemId}")]
  public IActionResult GetOneOrderProduct(string id)
  {
    if (!Guid.TryParse(id, out Guid OrderItemId_Guid))
    {
      return BadRequest("Invalid OrderProduct ID Format");
    }
    var OrderProduct = _orderProductService.GetOrderProductByIdService(OrderItemId_Guid);
    return Ok(OrderProduct);
  }

  [HttpPost]
  public async Task<IActionResult> CreateOrderProduct(OrderProduct newOrderProduct)
  {
    var createdOrderProduct = await _orderProductService.CreateOrderProductservice(newOrderProduct);
    return CreatedAtAction(nameof(GetOneOrderProduct), new { id = createdOrderProduct.OrderItemId }, createdOrderProduct);
  }

  [HttpPut("{OrderItemId}")]
  public IActionResult UpdateOrderProduct(string id, OrderProduct updateOrderProduct)
  {
    if (!Guid.TryParse(id, out Guid OrderItemId_Guid))
    {
      return BadRequest("Invalid OrderProduct ID Format");
    }
    var OrderProduct = _orderProductService.UpdateOrderProductservice(OrderItemId_Guid, updateOrderProduct);
    if (OrderProduct == null)
    {
      return NotFound();
    }
    return Ok(OrderProduct);
  }

  [HttpDelete("{OrderItemId}")]
  public async Task<IActionResult> DeleteOrderProduct(string id)
  {
    if (!Guid.TryParse(id, out Guid OrderItemId_Guid))
    {
      return BadRequest("Invalid OrderProduct ID Format");
    }
    var result = await _orderProductService.DeleteOrderProductservice(OrderItemId_Guid);
    if (!result)
    {
      return NotFound();
    }
    return NoContent();
  }
}
