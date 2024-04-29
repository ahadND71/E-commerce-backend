using Microsoft.AspNetCore.Http.HttpResults;

public class OrderService
{
  public static List<Order> _orders = new List<Order>(){
    new Order{
        OrderId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        TotalAmount = 1300
    },
    new Order{
        OrderId=  Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a999929"),
        TotalAmount = 200

    },
    new Order{
        OrderId =  Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3339"),
        TotalAmount = 600
    }
  };
  // SERVICES
  public IEnumerable<Order> GetAllOrderService()
  {
    return _orders;
  }
  public Order? GetOrderByIdService(Guid id)
  {
    return _orders.Find(Order => Order.OrderId == id);
  }
  public Order CreateOrderService(Order newOrder)
  {
    newOrder.OrderId = Guid.NewGuid();
    newOrder.CreatedAt = DateTime.Now;
    newOrder.UpdatedAt = DateTime.Now;

    _orders.Add(newOrder);
    return newOrder;
  }
  public Order? UpdateOrderService(Guid id, Order updateOrder)
  {

    var foundedOrder = _orders.FirstOrDefault(Order => Order.OrderId == id);
    if (foundedOrder != null)
    {
      foundedOrder.TotalAmount = updateOrder.TotalAmount;
      foundedOrder.Status = updateOrder.Status;
      foundedOrder.UpdatedAt = DateTime.Now;
    }
    return foundedOrder;
  }
  public bool DeleteOrderService(Guid id)
  {
    var OrderToRemove = _orders.FirstOrDefault(Order => Order.OrderId == id);
    if (OrderToRemove != null)
    {
      _orders.Remove(OrderToRemove);
      return true;
    }
    return false;
  }
}