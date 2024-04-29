using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Services;

public class OrderService
{
  private readonly AppDbContext _dbContext;

  public OrderService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }


  public async Task<IEnumerable<Order>> GetAllOrderService()
  {
    return await _dbContext.Orders.ToListAsync();
  }


  public async Task<Order?> GetOrderByIdService(Guid id)
  {
    return await _dbContext.Orders.FindAsync(id);
  }


  public async Task<Order> CreateOrderService(Order newOrder)
  {
    newOrder.OrderId = Guid.NewGuid();
    newOrder.CreatedAt = DateTime.Now;
    newOrder.UpdatedAt = DateTime.Now;
    _dbContext.Orders.Add(newOrder);
    await _dbContext.SaveChangesAsync();
    return newOrder;
  }

  public async Task<Order?> UpdateOrderService(Guid id, Order updateOrder)
  {
    var foundedOrder = await _dbContext.Orders.FindAsync(id);
    if (foundedOrder != null)
    {
      foundedOrder.TotalAmount = updateOrder.TotalAmount;
      foundedOrder.Status = updateOrder.Status;
      foundedOrder.UpdatedAt = DateTime.Now;
      await _dbContext.SaveChangesAsync();
    }
    return foundedOrder;
  }


  public async Task<bool> DeleteOrderService(Guid id)
  {
    var orderToRemove = await _dbContext.Orders.FindAsync(id);
    if (orderToRemove != null)
    {
      _dbContext.Orders.Remove(orderToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}
