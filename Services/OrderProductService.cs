using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

namespace api.Services;

public class OrderProductService
{
  private readonly AppDbContext _dbContext;
  public OrderProductService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }


  public async Task<PaginationResult<OrderProduct>> GetAllOrderProductService(int currentPage , int pageSize)
  {
    var totalOrderProductCount = await _dbContext.OrderProducts.CountAsync();
    var orderProduct = await _dbContext.OrderProducts
    .Skip((currentPage -1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
    return new PaginationResult<OrderProduct>{
      Items = orderProduct,
      TotalCount = totalOrderProductCount,
      CurrentPage = currentPage,
      PageSize = pageSize,
    };
  }


  public async Task<OrderProduct?> GetOrderProductByIdService(Guid orderItemId)
  {
    return await _dbContext.OrderProducts.FindAsync(orderItemId);
  }


  public async Task<OrderProduct> CreateOrderProductService(OrderProduct newOrderProduct)
  {
    newOrderProduct.OrderItemId = Guid.NewGuid();
    _dbContext.OrderProducts.Add(newOrderProduct);
    await _dbContext.SaveChangesAsync();
    return newOrderProduct;
  }


  public async Task<OrderProduct?> UpdateOrderProductService(Guid orderItemId, OrderProduct updateOrderProduct)
  {
    var existingOrderProduct = await _dbContext.OrderProducts.FindAsync(orderItemId);
    if (existingOrderProduct != null)
    {
      existingOrderProduct.Quantity = updateOrderProduct.Quantity ?? existingOrderProduct.Quantity;
      existingOrderProduct.ProductPrice = updateOrderProduct.ProductPrice;
      await _dbContext.SaveChangesAsync();
    }
    return existingOrderProduct;
  }


  public async Task<bool> DeleteOrderProductService(Guid orderItemId)
  {
    var orderProductToRemove = await _dbContext.OrderProducts.FindAsync(orderItemId);
    if (orderProductToRemove != null)
    {
      _dbContext.OrderProducts.Remove(orderProductToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}