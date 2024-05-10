using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;

namespace Backend.Controllers;


[ApiController]
[Route("/api/OrderProducts")]
public class OrderProductController : ControllerBase
{
  public OrderProductService _orderProductService;
  public OrderProductController(OrderProductService orderProductService)
  {
    _orderProductService = orderProductService;
  }


  [Authorize]
  [HttpGet]
  public async Task<IActionResult> GetAllOrderProducts([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
  {
    var orderProducts = await _orderProductService.GetAllOrderProductService(currentPage, pageSize);
    if (orderProducts.TotalCount < 1)
    {
      return ApiResponse.NotFound("No Orders Details To Display");
    }

    return ApiResponse.Success(
      orderProducts.Items,
      "Orders Details are returned successfully");
  }


  [Authorize]
  [HttpGet("{orderProductId}")]
  public async Task<IActionResult> GetOrderProduct(string orderProductId)
  {
    if (!Guid.TryParse(orderProductId, out Guid orderProductGuid))
    {
      return ApiResponse.BadRequest("Invalid OrderProduct ID Format");
    }

    var orderProduct = await _orderProductService.GetOrderProductByIdService(orderProductGuid);
    if (orderProduct == null)
    {
      return ApiResponse.NotFound(
        $"No Order Details Found With ID : ({orderProductGuid})");
    }
    else
    {
      return ApiResponse.Success(
        orderProduct,
        "Order Details is returned successfully"
      );
    }
  }


  [Authorize]
  [HttpPost]
  public async Task<IActionResult> CreateOrderProduct(OrderProduct newOrderProduct)
  {
    var createdOrderProduct = await _orderProductService.CreateOrderProductService(newOrderProduct);
    if (createdOrderProduct != null)
    {
      return ApiResponse.Created(createdOrderProduct, "Order details is created successfully");
    }
    else
    {
      return ApiResponse.ServerError("Error when creating new order details");
    }
  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{orderProductId}")]
  public async Task<IActionResult> UpdateOrderProduct(string orderProductId, OrderProductDto updateOrderProduct)
  {
    if (!Guid.TryParse(orderProductId, out Guid orderProductGuid))
    {
      return ApiResponse.BadRequest("Invalid Order Product ID Format");
    }

    var orderProduct = await _orderProductService.UpdateOrderProductService(orderProductGuid, updateOrderProduct);
    if (orderProduct == null)
    {
      return ApiResponse.NotFound("No Order Details Founded To Update");
    }
    return ApiResponse.Success(
      orderProduct,
      "Order Details Is Updated Successfully"
    );
  }


  [Authorize(Roles = "Admin")]
  [HttpDelete("{orderProductId}")]
  public async Task<IActionResult> DeleteOrderProduct(string orderProductId)
  {
    if (!Guid.TryParse(orderProductId, out Guid orderProductGuid))
    {
      return ApiResponse.BadRequest("Invalid OrderProduct ID Format");
    }

    var result = await _orderProductService.DeleteOrderProductService(orderProductGuid);
    if (!result)
    {
      return ApiResponse.NotFound("The Order Details is not found to be deleted");
    }
    return ApiResponse.Success(" Order Details is deleted successfully");
  }
}
