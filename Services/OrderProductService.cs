using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Services;

public class OrderProductService
{
  private readonly AppDbContext _dbContext;

  public OrderProductService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }


  public async Task<IEnumerable<OrderProduct>> GetAllOrderProductservice()
  {
    return await _dbContext.OrderProducts.ToListAsync();
  }


  public async Task<OrderProduct?> GetOrderProductByIdService(Guid id)
  {
    return await _dbContext.OrderProducts.FindAsync(id);
  }


  public async Task<OrderProduct> CreateOrderProductservice(OrderProduct newOrderProduct)
  {
    newOrderProduct.OrderItemId = Guid.NewGuid();
    _dbContext.OrderProducts.Add(newOrderProduct);
    await _dbContext.SaveChangesAsync();
    return newOrderProduct;
  }


  public async Task<OrderProduct?> UpdateOrderProductservice(Guid id, OrderProduct updateOrderProduct)
  {
    var foundedOrderProduct = await _dbContext.OrderProducts.FindAsync(id);
    if (foundedOrderProduct != null)
    {
      foundedOrderProduct.Quantity = updateOrderProduct.Quantity;
      foundedOrderProduct.ProductPrice = updateOrderProduct.ProductPrice;
      await _dbContext.SaveChangesAsync();
    }
    return foundedOrderProduct;
  }


  public async Task<bool> DeleteOrderProductservice(Guid id)
  {
    var orderProductToRemove = await _dbContext.OrderProducts.FindAsync(id);
    if (orderProductToRemove != null)
    {
      _dbContext.OrderProducts.Remove(orderProductToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }


}