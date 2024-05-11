using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;
using Backend.Dtos;

namespace Backend.Controllers;


[ApiController]
[Route("/api/orders")]
public class OrderController : ControllerBase
{
  public OrderService _orderService;
  public OrderController(OrderService orderService)
  {
    _orderService = orderService;
  }


  [Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<IActionResult> GetAllOrders([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
  {
    var orders = await _orderService.GetAllOrderService(currentPage, pageSize);
    if (orders.TotalCount < 1)
    {
      throw new NotFoundException("No Orders To Display");

    }
    return ApiResponse.Success(
              orders,
             "Orders are returned successfully");
  }


  [Authorize]
  [HttpGet("{orderId}")]
  public async Task<IActionResult> GetOrder(string orderId)
  {
    if (!Guid.TryParse(orderId, out Guid orderIdGuid))
    {
      throw new ValidationException("Invalid Order ID Format");
    }

    var order = await _orderService.GetOrderByIdService(orderIdGuid) ?? throw new NotFoundException($"No Order Found With ID : ({orderIdGuid})");

    return ApiResponse.Success<Order>(
      order,
      "Order is returned successfully"
    );

  }


  [Authorize]
  [HttpPost]
  public async Task<IActionResult> CreateOrder(Order newOrder)
  {
    var createdOrder = await _orderService.CreateOrderService(newOrder) ?? throw new Exception("Error when creating new order");

    return ApiResponse.Created(createdOrder, "Order is created successfully");

  }


  [Authorize(Roles = "Admin")]
  [HttpPut("{orderId}")]
  public async Task<IActionResult> UpdateOrder(string orderId, OrderDto updateOrder)
  {
    if (!Guid.TryParse(orderId, out Guid orderIdGuid))
    {
      throw new ValidationException("Invalid Order ID Format");
    }

    var order = await _orderService.UpdateOrderService(orderIdGuid, updateOrder) ?? throw new NotFoundException("No Order Founded To Update");

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
      throw new ValidationException("Invalid order ID Format");
    }

    var result = await _orderService.DeleteOrderService(OrderId_Guid);
    if (!result)
    {
      throw new NotFoundException("The Order is not found to be deleted");

    }
    return ApiResponse.Success(" Order is deleted successfully");
  }
}
