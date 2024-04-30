using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.Net.Http.Headers;

namespace api.Services;

public class CustomerService
{

  private readonly AppDbContext _dbContext;
  public CustomerService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<IEnumerable<Customer>> GetAllCustomersService()
  {
    return await _dbContext.Customers.ToListAsync();
  }


  public async Task<Customer?> GetCustomerById(Guid customerId)
  {
    return await _dbContext.Customers.FindAsync(customerId);
  }


  public async Task<Customer> CreateCustomerService(Customer newCustomer)
  {
    newCustomer.CustomerId = Guid.NewGuid();
    newCustomer.CreatedAt = DateTime.Now;
    _dbContext.Customers.Add(newCustomer);
    await _dbContext.SaveChangesAsync();
    return newCustomer;
  }


  public async Task<Customer> UpdateCustomerService(Guid customerId, Customer updateCustomer)
  {
    var existingCustomer = await _dbContext.Customers.FindAsync(customerId);
    if (existingCustomer != null)
    {
      existingCustomer.FirstName = updateCustomer.FirstName ?? existingCustomer.FirstName;
      existingCustomer.LastName = updateCustomer.LastName ?? existingCustomer.LastName;
      existingCustomer.Email = updateCustomer.Email ?? existingCustomer.Email;
      existingCustomer.Password = updateCustomer.Password ?? existingCustomer.Password;
      existingCustomer.Address = updateCustomer.Address ?? existingCustomer.Address;
      existingCustomer.Image = updateCustomer.Image ?? existingCustomer.Image;
      existingCustomer.IsBanned = updateCustomer.IsBanned;
      await _dbContext.SaveChangesAsync();
    }
    return existingCustomer;
  }


  public async Task<bool> DeleteCustomerService(Guid customerId)
  {
    var customerToRemove = await _dbContext.Customers.FindAsync(customerId);
    if (customerToRemove != null)
    {
      _dbContext.Customers.Remove(customerToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}