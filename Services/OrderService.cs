using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using Backend.Dtos;

namespace Backend.Services;

public class OrderService
{
    private readonly AppDbContext _dbContext;

    public OrderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PaginationResult<Order>> GetAllOrderService(int currentPage, int pageSize)
    {
        var totalOrderCount = await _dbContext.Orders.CountAsync();
        var order = await _dbContext.Orders.Include(op => op.OrderProducts)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginationResult<Order>
        {
            Items = order,
            TotalCount = totalOrderCount,
            CurrentPage = currentPage,
            PageSize = pageSize,
        };
    }


    public async Task<Order?> GetOrderByIdService(Guid orderId)
    {
        return await _dbContext.Orders.Include(op => op.OrderProducts).FirstOrDefaultAsync(o => o.OrderId == orderId);
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


    public async Task<Order?> UpdateOrderService(Guid orderId, OrderDto updateOrder)
    {
        var existingOrder = await _dbContext.Orders.FindAsync(orderId);
        if (existingOrder != null)
        {
            existingOrder.TotalPrice = updateOrder.TotalPrice ?? existingOrder.TotalPrice;
            existingOrder.Status = updateOrder.Status.IsNullOrEmpty() ? existingOrder.Status : updateOrder.Status;
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