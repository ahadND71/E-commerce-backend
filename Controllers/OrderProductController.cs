using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Dtos;
using SendGrid.Helpers.Errors.Model;

namespace Backend.Controllers;

[ApiController]
[Route("/api/OrderProducts")]
public class OrderProductController : ControllerBase
{
    private readonly OrderProductService _orderProductService;

    public OrderProductController(OrderProductService orderProductService)
    {
        _orderProductService = orderProductService;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllOrderProducts([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var orderProducts = await _orderProductService.GetAllOrderProductService(currentPage, pageSize);
        if (orderProducts.TotalCount < 1)
        {
            throw new NotFoundException("No Orders Details To Display");
        }

        return ApiResponse.Success(
            orderProducts,
            "Orders Details are returned successfully");
    }


    [Authorize]
    [HttpGet("{orderProductId}")]
    public async Task<IActionResult> GetOrderProduct(string orderProductId)
    {
        if (!Guid.TryParse(orderProductId, out Guid orderProductGuid))
        {
            throw new ValidationException("Invalid OrderProduct ID Format");
        }

        var orderProduct = await _orderProductService.GetOrderProductByIdService(orderProductGuid) ?? throw new NotFoundException($"No Order Details Found With ID : ({orderProductGuid})");

        return ApiResponse.Success(
            orderProduct,
            "Order Details is returned successfully"
        );
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrderProduct(OrderProduct newOrderProduct)
    {
        var createdOrderProduct = await _orderProductService.CreateOrderProductService(newOrderProduct) ?? throw new Exception("Error when creating new order details");

        return ApiResponse.Created(createdOrderProduct, "Order details is created successfully");
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{orderProductId}")]
    public async Task<IActionResult> UpdateOrderProduct(string orderProductId, OrderProductDto updateOrderProduct)
    {
        if (!Guid.TryParse(orderProductId, out Guid orderProductGuid))
        {
            throw new ValidationException("Invalid Order Product ID Format");
        }

        var orderProduct = await _orderProductService.UpdateOrderProductService(orderProductGuid, updateOrderProduct) ?? throw new NotFoundException("No Order Details Founded To Update");

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
            throw new ValidationException("Invalid OrderProduct ID Format");
        }

        var result = await _orderProductService.DeleteOrderProductService(orderProductGuid);
        if (!result)
        {
            throw new NotFoundException("The Order Details is not found to be deleted");
        }

        return ApiResponse.Success(" Order Details is deleted successfully");
    }
}