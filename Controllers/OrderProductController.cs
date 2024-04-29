using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [ApiController]
  [Route("/api/OrderProducts")]
  public class OrderProductController : ControllerBase
  {
    public OrderProductService _orderProductService;
    public OrderProductController()
    {
      _orderProductService = new OrderProductService();
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
    public IActionResult CreateOrderProduct(OrderProduct newOrderProduct)
    {
      var createdOrderProduct = _orderProductService.CreateOrderProductservice(newOrderProduct);
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
    public IActionResult DeleteOrderProduct(string id)
    {
      if (!Guid.TryParse(id, out Guid OrderItemId_Guid))
      {
        return BadRequest("Invalid OrderProduct ID Format");
      }
      var result = _orderProductService.DeleteOrderProductservice(OrderItemId_Guid);
      if (!result)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}