public class CustomerService
{
  public static List<Customer> _customers = new List<Customer>() {
    new Customer{
        CustomerId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        FirstName = "John",
        LastName = "Doe",
        Email = "john@example.com",
        Password = "password123",
        Address = "123 Main St",
        IsBanned = false,
        CreatedAt = DateTime.Now
    },
    new Customer{
        CustomerId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        FirstName = "Alice",
        LastName = "Smith",
        Email = "alice@example.com",
        Password = "password456",
        Address = "456 Elm St",
        IsBanned = false,
        CreatedAt = DateTime.Now
    },
    new Customer{
        CustomerId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        FirstName = "Bob",
        LastName = "Johnson",
        Email = "bob@example.com",
        Password = "password789",
        Address = "789 Oak St",
        IsBanned = false,
        CreatedAt = DateTime.Now
    }
  };


  public IEnumerable<Customer> GetAllCustomersService()
  {
    return _customers;
  }


  public Customer? GetCustomerById(Guid customerId)
  {
    return _customers.Find(customer => customer.CustomerId == customerId);
  }


  public Customer CreateCustomerService(Customer newCustomer)
  {
    newCustomer.CustomerId = Guid.NewGuid();
    newCustomer.CreatedAt = DateTime.Now;
    _customers.Add(newCustomer);
    return newCustomer;
  }


  public Customer UpdateCustomerService(Guid customerId, Customer updateCustomer)
  {
    var existingCustomer = _customers.FirstOrDefault(u => u.CustomerId == customerId);
    if (existingCustomer != null)
    {
      existingCustomer.FirstName = updateCustomer.FirstName;
      existingCustomer.LastName = updateCustomer.LastName;
      existingCustomer.Email = updateCustomer.Email;
      existingCustomer.Password = updateCustomer.Password;
      existingCustomer.Address = updateCustomer.Address;
      existingCustomer.Image = updateCustomer.Image;
      existingCustomer.IsBanned = updateCustomer.IsBanned;
    }
    return existingCustomer;
  }


  public bool DeleteCustomerService(Guid customerId)
  {
    var customerToRemove = _customers.FirstOrDefault(u => u.CustomerId == customerId);
    if (customerToRemove != null)
    {
      _customers.Remove(customerToRemove);
      return true;
    }
    return false;
  }

}