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
    return await _dbContext.Orders.Include(op => op.OrderProducts).ToListAsync();
  }


  public async Task<Order?> GetOrderByIdService(Guid orderId)
  {
    return await _dbContext.Orders.FindAsync(orderId);
  }


  public async Task<Order> CreateOrderService(Order newOrder)
  {
    newOrder.OrderId = Guid.NewGuid();
    newOrder.CreatedAt = DateTime.UtcNow;
    newOrder.UpdatedAt = DateTime.UtcNow;
    _dbContext.Orders.Add(newOrder);
    await _dbContext.SaveChangesAsync();
    return newOrder;
  }

  public async Task<Order?> UpdateOrderService(Guid orderId, Order updateOrder)
  {
    var existingOrder = await _dbContext.Orders.FindAsync(orderId);
    if (existingOrder != null)
    {
      existingOrder.TotalAmount = updateOrder.TotalAmount;
      existingOrder.Status = updateOrder.Status ?? existingOrder.Status;
      existingOrder.UpdatedAt = DateTime.UtcNow;
      await _dbContext.SaveChangesAsync();
    }
    return existingOrder;
  }


  public async Task<bool> DeleteOrderService(Guid orderId)
  {
    var orderToRemove = await _dbContext.Orders.FindAsync(orderId);
    if (orderToRemove != null)
    {
      _dbContext.Orders.Remove(orderToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}
