using Microsoft.AspNetCore.Authorization;
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


  [Authorize]
  [HttpGet]
  public async Task<IActionResult> GetAllOrders([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
  {
    var orders = await _orderService.GetAllOrderService(currentPage, pageSize);
    if (orders.TotalCount < 1)
    {
      return ApiResponse.NotFound("No Orders To Display");

    }
    return ApiResponse.Success<IEnumerable<Order>>(
              orders.Items,
             "Orders are returned successfully");
  }


  [Authorize]
  [HttpGet("{orderId}")]
  public async Task<IActionResult> GetOrder(string orderId)
  {
    if (!Guid.TryParse(orderId, out Guid orderIdGuid))
    {
      return ApiResponse.BadRequest("Invalid Order ID Format");
    }

    var order = await _orderService.GetOrderByIdService(orderIdGuid);
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


  [Authorize]
  [HttpPost]
  public async Task<IActionResult> CreateOrder(Order newOrder)
  {
    var createdOrder = await _orderService.CreateOrderService(newOrder);
    if (createdOrder != null)
    {
      return ApiResponse.Created(createdOrder, "Order is created successfully");
    }
    else
    {
      return ApiResponse.ServerError("Error when creating new order");

    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{orderId}")]
  public async Task<IActionResult> UpdateOrder(string orderId, Order updateOrder)
  {
    if (!Guid.TryParse(orderId, out Guid orderIdGuid))
    {
      return ApiResponse.BadRequest("Invalid Order ID Format");
    }

    var order = await _orderService.UpdateOrderService(orderIdGuid, updateOrder);
    if (order == null)
    {
      return ApiResponse.NotFound("No Order Founded To Update");
    }
    return ApiResponse.Success(
        order,
        "Order Is Updated Successfully"
    );
  }


  [Authorize(Roles = "Admin")]
  [HttpDelete("{orderId}")]
  public async Task<IActionResult> DeleteOrder(string orderId)
  {
    if (!Guid.TryParse(orderId, out Guid OrderId_Guid))
    {
      return ApiResponse.BadRequest("Invalid order ID Format");
    }

    var result = await _orderService.DeleteOrderService(OrderId_Guid);
    if (!result)
    {
      return ApiResponse.NotFound("The Order is not found to be deleted");

    }
    return ApiResponse.Success(" Order is deleted successfully");
  }
}
