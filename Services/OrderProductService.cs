using Microsoft.AspNetCore.Http.HttpResults;

public class OrderProductService
{
  public static List<OrderProduct> _orderProducts = new List<OrderProduct>(){
    new OrderProduct{
        OrderItemId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        Quantity = 2,
        ProductPrice = 200,
    },
    new OrderProduct{
        OrderItemId=  Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a999929"),
        Quantity = 8,
        ProductPrice = 150,

    },
    new OrderProduct{
        OrderItemId =  Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3339"),
        Quantity = 5,
        ProductPrice = 180,
    }
  };
  // SERVICES
  public IEnumerable<OrderProduct> GetAllOrderProductservice()
  {
    return _orderProducts;
  }
  public OrderProduct? GetOrderProductByIdService(Guid id)
  {
    return _orderProducts.Find(orderProduct => orderProduct.OrderItemId == id);
  }
  public OrderProduct CreateOrderProductservice(OrderProduct newOrderProduct)
  {
    newOrderProduct.OrderItemId = Guid.NewGuid();

    _orderProducts.Add(newOrderProduct);
    return newOrderProduct;
  }
  public OrderProduct? UpdateOrderProductservice(Guid id, OrderProduct updateOrderProduct)
  {

    var foundedOrderProduct = _orderProducts.FirstOrDefault(orderProduct => orderProduct.OrderItemId == id);
    if (foundedOrderProduct != null)
    {
      foundedOrderProduct.Quantity = updateOrderProduct.Quantity;
      foundedOrderProduct.ProductPrice = updateOrderProduct.ProductPrice;
    }
    return foundedOrderProduct;
  }
  public bool DeleteOrderProductservice(Guid id)
  {
    var orderProductToRemove = _orderProducts.FirstOrDefault(orderProduct => orderProduct.OrderItemId == id);
    if (orderProductToRemove != null)
    {
      _orderProducts.Remove(orderProductToRemove);
      return true;
    }
    return false;
  }
}