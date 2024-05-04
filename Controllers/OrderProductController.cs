using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using api.Authentication.Identity;

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


  [AllowAnonymous]
  [HttpGet]
  public async Task<IActionResult> GetAllOrderProducts()
  {
    try
    {
      var orderProducts = await _dbContext.GetAllOrderProductService();
      if (orderProducts.ToList().Count < 1)
      {
        return NotFound(new ErrorMessage
        {
          Message = "No Order Details To Display"
        });
      }
      return Ok(new SuccessMessage<IEnumerable<OrderProduct>>
      {
        Message = "Orders Details are returned succeSSfully",
        Data = orderProducts
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Order Detail list");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [AllowAnonymous]
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
          Message = "Order Details is returned succeSSfully",
          Data = orderProduct
        });
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Order Details");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [Authorize]
  [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
  [HttpPost]
  public async Task<IActionResult> CreateOrderProduct(OrderProduct newOrderProduct)
  {
    try
    {
      var createdOrderProduct = await _dbContext.CreateOrderProductService(newOrderProduct);
      if (createdOrderProduct != null)
      {
        return CreatedAtAction(nameof(GetOrderProduct), new { orderItemId = createdOrderProduct.OrderItemId }, createdOrderProduct);
      }
      return Ok(new SuccessMessage<OrderProduct>
      {
        Message = "Order Details is created successfully",
        Data = createdOrderProduct
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot create new Order Details");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [Authorize]
  [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
  [HttpPut("{orderItemId}")]
  public async Task<IActionResult> UpdateOrderProduct(string orderItemId, OrderProduct updateOrderProduct)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid))
      {
        return BadRequest("Invalid OrderProduct ID Format");
      }
      var orderProduct = await _dbContext.UpdateOrderProductService(orderItemIdGuid, updateOrderProduct);
      if (orderProduct == null)

      {
        return NotFound(new ErrorMessage
        {
          Message = "No Order Details To Founded To Update"
        });
      }
      return Ok(new SuccessMessage<OrderProduct>
      {
        Message = "Order Details Is Updated Successfully",
        Data = orderProduct
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot update the Order Details ");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }


  [Authorize]
  [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
  [HttpDelete("{OrderItemId}")]
  public async Task<IActionResult> DeleteOrderProduct(string orderItemId)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid OrderItemId_Guid))
      {
        return BadRequest("Invalid OrderProduct ID Format");
      }
      var result = await _dbContext.DeleteOrderProductService(OrderItemId_Guid);
      if (!result)
      {
        return NotFound(new ErrorMessage
        {
          Message = "The Order Details is not found to be deleted"
        });
      }
      return Ok(new { success = true, message = " Order Details is deleted successfully" });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, the Order Details can not deleted");
      return StatusCode(500, new ErrorMessage
      {
        Message = ex.Message
      });
    }
  }
}
