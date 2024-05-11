using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Dtos;

namespace Backend.Services;

public class OrderProductService
{
    private readonly AppDbContext _dbContext;

    public OrderProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PaginationResult<OrderProduct>> GetAllOrderProductService(int currentPage, int pageSize)
    {
        var totalOrderProductCount = await _dbContext.OrderProducts.CountAsync();
        var orderProduct = await _dbContext.OrderProducts
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
        return await _dbContext.OrderProducts.FindAsync(orderItemId);
    }


    public async Task<OrderProduct> CreateOrderProductService(OrderProduct newOrderProduct)
    {
        newOrderProduct.OrderProductId = Guid.NewGuid();
        var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == newOrderProduct.ProductId);
        if (product != null)
        {
            newOrderProduct.ProductPrice = product.Price * newOrderProduct.Quantity;

            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == newOrderProduct.OrderId);

            order.TotalPrice += newOrderProduct.ProductPrice;
        }

        _dbContext.OrderProducts.Add(newOrderProduct);
        await _dbContext.SaveChangesAsync();
        return newOrderProduct;
    }


    public async Task<OrderProduct?> UpdateOrderProductService(Guid orderItemId, OrderProductDto updateOrderProduct)
    {
        var existingOrderProduct = await _dbContext.OrderProducts.FindAsync(orderItemId);
        if (existingOrderProduct != null)
        {
            existingOrderProduct.Quantity = updateOrderProduct.Quantity ?? existingOrderProduct.Quantity;
            existingOrderProduct.ProductPrice = updateOrderProduct.ProductPrice ?? existingOrderProduct.ProductPrice;
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