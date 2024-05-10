using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using Backend.Dtos;

namespace Backend.Services;

public class OrderService
{
    private readonly AppDbContext _orderDbContext;

    public OrderService(AppDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }


    public async Task<PaginationResult<Order>> GetAllOrderService(int currentPage, int pageSize)
    {
        var totalOrderCount = await _orderDbContext.Orders.CountAsync();
        var order = await _orderDbContext.Orders.Include(op => op.OrderProducts)
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
        return await _orderDbContext.Orders.Include(op => op.OrderProducts).FirstOrDefaultAsync(o => o.OrderId == orderId);
    }


    public async Task<Order> CreateOrderService(Order newOrder)
    {
        newOrder.OrderId = Guid.NewGuid();
        newOrder.CreatedAt = DateTime.UtcNow;
        newOrder.UpdatedAt = DateTime.UtcNow;
        _orderDbContext.Orders.Add(newOrder);
        await _orderDbContext.SaveChangesAsync();
        return newOrder;
    }


    public async Task<Order?> UpdateOrderService(Guid orderId, OrderDto updateOrder)
    {
        var existingOrder = await _orderDbContext.Orders.FindAsync(orderId);
        if (existingOrder != null)
        {
            existingOrder.TotalPrice = updateOrder.TotalPrice ?? existingOrder.TotalPrice;
            existingOrder.Status = updateOrder.Status.IsNullOrEmpty() ? existingOrder.Status : updateOrder.Status;
            existingOrder.UpdatedAt = DateTime.UtcNow;
            await _orderDbContext.SaveChangesAsync();
        }

        return existingOrder;
    }


    public async Task<bool> DeleteOrderService(Guid orderId)
    {
        var orderToRemove = await _orderDbContext.Orders.FindAsync(orderId);
        if (orderToRemove != null)
        {
            _orderDbContext.Orders.Remove(orderToRemove);
            await _orderDbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}