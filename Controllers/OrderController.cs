using Microsoft.AspNetCore.Mvc;
using api.Services;

namespace api.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrderController : ControllerBase
{
  public OrderService _orderService;
  public OrderController(OrderService orderService)
  {
    _orderService = orderService;
  }

  [HttpGet]
  public IActionResult GetAllOrders()
  {
    var Orders = _orderService.GetAllOrderService();
    return Ok(Orders);
  }

  [HttpGet("{id}")]
  public IActionResult GetOneOrder(string id)
  {
    if (!Guid.TryParse(id, out Guid OrderId_Guid))
    {
      return BadRequest("Invalid Order ID Format");
    }
    var Order = _orderService.GetOrderByIdService(OrderId_Guid);
    return Ok(Order);
  }

  [HttpPost]
  public async Task<IActionResult> CreateOrder(Order newOrder)
  {
    var createdOrder = await _orderService.CreateOrderService(newOrder);
    return CreatedAtAction(nameof(GetOneOrder), new { id = createdOrder.OrderId }, createdOrder);
  }

  [HttpPut("{id}")]
  public IActionResult UpdateOrder(string id, Order updateOrder)
  {
    if (!Guid.TryParse(id, out Guid OrderId_Guid))
    {
      return BadRequest("Invalid Order ID Format");
    }
    var Order = _orderService.UpdateOrderService(OrderId_Guid, updateOrder);
    if (Order == null)
    {
      return NotFound();
    }
    return Ok(Order);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteOrder(string id)
  {
    if (!Guid.TryParse(id, out Guid OrderId_Guid))
    {
      return BadRequest("Invalid Order ID Format");
    }
    var result = await _orderService.DeleteOrderService(OrderId_Guid);
    if (!result)
    {
      return NotFound();
    }
    return NoContent();
  }
}
