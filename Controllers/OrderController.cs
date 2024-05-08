using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;

namespace api.Controllers;


[ApiController]
[Route("/api/orders")]
public class OrderController : ControllerBase
{
  public OrderService _dbContext;
  public OrderController(OrderService orderService)
  {
    _dbContext = orderService;
  }


  [Authorize]
  [HttpGet]
  public async Task<IActionResult> GetAllOrders()
  {
    try
    {
      var orders = await _dbContext.GetAllOrderService();
      if (orders.ToList().Count < 1)
      {
        return ApiResponse.NotFound("No Orders To Display");

      }
      return ApiResponse.Success<IEnumerable<Order>>(
                orders,
               "Orders are returned successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Order list");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize]
  [HttpGet("{orderId}")]
  public async Task<IActionResult> GetOrder(string orderId)
  {
    try
    {
      if (!Guid.TryParse(orderId, out Guid orderIdGuid))
      {
        return ApiResponse.BadRequest("Invalid Order ID Format");
      }
      var order = await _dbContext.GetOrderByIdService(orderIdGuid);
      if (order == null)
      {
        return ApiResponse.NotFound(
                  $"No Order Found With ID : ({orderIdGuid})");
      }
      else
      {
        return ApiResponse.Success<Order>(
          order,
          "Order is returned successfully"
        );
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot return the Order");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize]
  [HttpPost]
  public async Task<IActionResult> CreateOrder(Order newOrder)
  {
    try
    {
      var createdOrder = await _dbContext.CreateOrderService(newOrder);
      if (createdOrder != null)
      {
        return ApiResponse.Created<Order>(createdOrder, "Order is created successfully");
      }
      else
      {
        return ApiResponse.ServerError("Error when creating new order");

      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot create new Order");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{orderId}")]
  public async Task<IActionResult> UpdateOrder(string orderId, Order updateOrder)
  {
    try
    {
      if (!Guid.TryParse(orderId, out Guid orderIdGuid))
      {
        return ApiResponse.BadRequest("Invalid Order ID Format");
      }
      var order = await _dbContext.UpdateOrderService(orderIdGuid, updateOrder);
      if (order == null)
      {
        return ApiResponse.NotFound("No Order Founded To Update");
      }
      return ApiResponse.Success<Order>(
          order,
          "Order Is Updated Successfully"
      );
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, cannot update the Order ");
      return ApiResponse.ServerError(ex.Message);

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpDelete("{orderId}")]
  public async Task<IActionResult> DeleteOrder(string orderId)
  {
    try
    {
      if (!Guid.TryParse(orderId, out Guid OrderId_Guid))
      {
        return ApiResponse.BadRequest("Invalid order ID Format");
      }
      var result = await _dbContext.DeleteOrderService(OrderId_Guid);
      if (!result)
      {
        return ApiResponse.NotFound("The Order is not found to be deleted");

      }
      return ApiResponse.Success(" Order is deleted successfully");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred, the Order can not deleted");
      return ApiResponse.ServerError(ex.Message);

    }
  }
}
