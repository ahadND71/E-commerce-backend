using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

namespace api.Services;

public class OrderProductService
{
  private readonly AppDbContext _orderProductDbContext;
  public OrderProductService(AppDbContext orderProductDbContext)
  {
    _orderProductDbContext = orderProductDbContext;
  }


  public async Task<PaginationResult<OrderProduct>> GetAllOrderProductService(int currentPage, int pageSize)
  {
    var totalOrderProductCount = await _orderProductDbContext.OrderProducts.CountAsync();
    var orderProduct = await _orderProductDbContext.OrderProducts
    .Skip((currentPage - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
    return new PaginationResult<OrderProduct>
    {
      Items = orderProduct,
      TotalCount = totalOrderProductCount,
      CurrentPage = currentPage,
      PageSize = pageSize,
    };
  }


  public async Task<OrderProduct?> GetOrderProductByIdService(Guid orderItemId)
  {
    return await _orderProductDbContext.OrderProducts.FindAsync(orderItemId);
  }


  public async Task<OrderProduct> CreateOrderProductService(OrderProduct newOrderProduct)
  {
    newOrderProduct.OrderProductId = Guid.NewGuid();
    _orderProductDbContext.OrderProducts.Add(newOrderProduct);
    await _orderProductDbContext.SaveChangesAsync();
    return newOrderProduct;
  }


  public async Task<OrderProduct?> UpdateOrderProductService(Guid orderItemId, OrderProduct updateOrderProduct)
  {
    var existingOrderProduct = await _orderProductDbContext.OrderProducts.FindAsync(orderItemId);
    if (existingOrderProduct != null)
    {
      existingOrderProduct.Quantity = updateOrderProduct.Quantity ?? existingOrderProduct.Quantity;
      existingOrderProduct.ProductPrice = updateOrderProduct.ProductPrice;
      await _orderProductDbContext.SaveChangesAsync();
    }
    return existingOrderProduct;
  }


  public async Task<bool> DeleteOrderProductService(Guid orderItemId)
  {
    var orderProductToRemove = await _orderProductDbContext.OrderProducts.FindAsync(orderItemId);
    if (orderProductToRemove != null)
    {
      _orderProductDbContext.OrderProducts.Remove(orderProductToRemove);
      await _orderProductDbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}