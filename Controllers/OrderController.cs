using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [ApiController]
  [Route("/api/orders")]
  public class OrderController : ControllerBase
  {
    public OrderService _orderService;
    public OrderController()
    {
      _orderService = new OrderService();
    }

    [HttpGet]
    public IActionResult GetAllOrders()
    {
      var Orders = _orderService.GetAllOrderService();
      return Ok(Orders);
    }

    [HttpGet("{OrderId}")]
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
    public IActionResult CreateOrder(Order newOrder)
    {
      var createdOrder = _orderService.CreateOrderService(newOrder);
      return CreatedAtAction(nameof(GetOneOrder), new { id = createdOrder.OrderId }, createdOrder);
    }

    [HttpPut("{OrderId}")]
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

    [HttpDelete("{OrderId}")]
    public IActionResult DeleteOrder(string id)
    {
      if (!Guid.TryParse(id, out Guid OrderId_Guid))
      {
        return BadRequest("Invalid Order ID Format");
      }
      var result = _orderService.DeleteOrderService(OrderId_Guid);
      if (!result)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}