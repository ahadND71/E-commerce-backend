using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Helpers;

namespace api.Controllers;

[ApiController]
[Route("/api/OrderProducts")]
public class OrderProductController : ControllerBase
{
  public OrderProductService _dbContext;
  public OrderProductController(OrderProductService orderProductService)
  {
    _dbContext = orderProductService;
  }


  [HttpGet]
  public async Task<IActionResult> GetAllOrderProducts()
  {
    try
    {

      var orderProducts = await _dbContext.GetAllOrderProductservice();
      if (orderProducts.ToList().Count < 1)
      {
        return NotFound(new ErrorMessage
        {
          Message = "No Order Details To Display"
        });
      }
      return Ok(new SuccessMessage<IEnumerable<OrderProduct>>
      {
        Message = "Oreders Details are returned succeefully",
        Data = orderProducts
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not return the Oreder Detail list");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpGet("{orderItemId}")]
  public async Task<IActionResult> GetOrderProduct(string orderItemId)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid))
      {
        return BadRequest("Invalid OrderProduct ID Format");
      }
      var orderProduct = await _dbContext.GetOrderProductByIdService(orderItemIdGuid);
      if (orderProduct == null)

      {
        return NotFound(new ErrorMessage
        {
          Message = $"No Order Details Found With ID : ({orderItemIdGuid})"
        });
      }
      else
      {
        return Ok(new SuccessMessage<OrderProduct>
        {
          Success = true,
          Message = "Order Details is returned succeefully",
          Data = orderProduct
        });
      }

    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not return the Order Details");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpPost]
  public async Task<IActionResult> CreateOrderProduct(OrderProduct newOrderProduct)
  {
    try
    {

      var createdOrderProduct = await _dbContext.CreateOrderProductservice(newOrderProduct);
      if (createdOrderProduct != null)
      {
        return CreatedAtAction(nameof(GetOrderProduct), new { orderItemId = createdOrderProduct.OrderItemId }, createdOrderProduct);
      }
      return Ok(new SuccessMessage<OrderProduct>
      {
        Message = "Order Details is created succeefully",
        Data = createdOrderProduct
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not create new Order Details");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpPut("{orderItemId}")]
  public async Task<IActionResult> UpdateOrderProduct(string orderItemId, OrderProduct updateOrderProduct)
  {
    try
    {

      if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid))
      {
        return BadRequest("Invalid OrderProduct ID Format");
      }
      var orderProduct = await _dbContext.UpdateOrderProductservice(orderItemIdGuid, updateOrderProduct);
      if (orderProduct == null)

      {
        return NotFound(new ErrorMessage
        {
          Message = "No Order Details To Founed To Update"
        });
      }
      return Ok(new SuccessMessage<OrderProduct>
      {
        Message = "Order Details Is Updated Succeefully",
        Data = orderProduct
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , can not update the Order Details ");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [HttpDelete("{OrderItemId}")]
  public async Task<IActionResult> DeleteOrderProduct(string orderItemId)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid OrderItemId_Guid))
      {
        return BadRequest("Invalid OrderProduct ID Format");
      }
      var result = await _dbContext.DeleteOrderProductservice(OrderItemId_Guid);
      if (!result)


      {
        return NotFound(new ErrorMessage
        {
          Message = "The Order Details is not found to be deleted"
        });
      }
      return Ok(new { success = true, message = " Order Details is deleted succeefully" });
    }

    catch (Exception ex)
    {
      Console.WriteLine($"An error occured , the Order Details can not deleted");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }
}
