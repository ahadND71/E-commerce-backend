using Microsoft.AspNetCore.Authorization;
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


  [Authorize]
  [HttpGet]
  public async Task<IActionResult> GetAllOrderProducts([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
  {
    try
    {
      var orderProducts = await _dbContext.GetAllOrderProductService(currentPage , pageSize);
      if (orderProducts.TotalCount < 1)
      {
        return ApiResponse.NotFound("No Orders Details To Display");

      }
      return ApiResponse.Success<IEnumerable<OrderProduct>>(
                orderProducts.Items,
               "Orders Details are returned successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Order Detail list");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize]
  [HttpGet("{orderItemId}")]
  public async Task<IActionResult> GetOrderProduct(string orderItemId)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid))
      {
        return ApiResponse.BadRequest("Invalid OrderProduct ID Format");
      }
      var orderProduct = await _dbContext.GetOrderProductByIdService(orderItemIdGuid);
      if (orderProduct == null)

      {
        return ApiResponse.NotFound(
                $"No Order Details Found With ID : ({orderItemIdGuid})");
      }

      else
      {
        return ApiResponse.Success<OrderProduct>(
                  orderProduct,
                  "Order Details is returned successfully"
                );
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Order Details");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize]
  [HttpPost]
  public async Task<IActionResult> CreateOrderProduct(OrderProduct newOrderProduct)
  {
    try
    {
      var createdOrderProduct = await _dbContext.CreateOrderProductService(newOrderProduct);
      if (createdOrderProduct != null)
      {
        return ApiResponse.Created<OrderProduct>(createdOrderProduct, "Order details is created successfully");
      }
      else
      {
        return ApiResponse.ServerError("Error when creating new order details");

      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot create new Order Details");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{orderItemId}")]
  public async Task<IActionResult> UpdateOrderProduct(string orderItemId, OrderProduct updateOrderProduct)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid))
      {
        return ApiResponse.BadRequest("Invalid Order Product ID Format");
      }
      var orderProduct = await _dbContext.UpdateOrderProductService(orderItemIdGuid, updateOrderProduct);
      if (orderProduct == null)
      {

        return ApiResponse.NotFound("No Order Details Founded To Update");
      }
      return ApiResponse.Success<OrderProduct>(
          orderProduct,
          "Order Details Is Updated Successfully"
      );
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot update the Order Details ");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpDelete("{OrderItemId}")]
  public async Task<IActionResult> DeleteOrderProduct(string orderItemId)
  {
    try
    {
      if (!Guid.TryParse(orderItemId, out Guid OrderItemId_Guid))
      {
        return ApiResponse.BadRequest("Invalid OrderProduct ID Format");
      }
      var result = await _dbContext.DeleteOrderProductService(OrderItemId_Guid);
      if (!result)
      {
        return ApiResponse.NotFound("The Order Details is not found to be deleted");
      }
      return ApiResponse.Success(" Order Details is deleted successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, the Order Details can not deleted");
      return ApiResponse.ServerError(ex.Message);

    }
  }
}
