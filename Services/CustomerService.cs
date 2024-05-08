using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.AspNetCore.Identity;
using api.Authentication.Dtos;

namespace api.Services;

public class CustomerService
{

  private readonly AppDbContext _dbContext;
  private readonly IPasswordHasher<Customer> _passwordHasher;

  public CustomerService(AppDbContext dbContext, IPasswordHasher<Customer> passwordHasher)
  {
    _dbContext = dbContext;
    _passwordHasher = passwordHasher;
  }


  public async Task<IEnumerable<Customer>> GetAllCustomersService()
  {
    return await _dbContext.Customers
    .Include(a => a.Addresses)
    .Include(o => o.Orders)
      .ThenInclude(op => op.OrderProducts)
    .Include(r => r.Reviews)
    .ToListAsync();
  }


  public async Task<Customer?> GetCustomerById(Guid customerId)
  {
    return await _dbContext.Customers.FindAsync(customerId);
  }


  public async Task<Customer> CreateCustomerService(Customer newCustomer)
  {
    newCustomer.CustomerId = Guid.NewGuid();
    newCustomer.CreatedAt = DateTime.UtcNow;
    newCustomer.Password = _passwordHasher.HashPassword(newCustomer, newCustomer.Password);
    _dbContext.Customers.Add(newCustomer);
    await _dbContext.SaveChangesAsync();
    return newCustomer;
  }

  public async Task<LoginUserDto?> LoginCustomerService(LoginUserDto loginUserDto)
  {
    var customer = await _dbContext.Customers.SingleOrDefaultAsync(c => c.Email == loginUserDto.Email);
    if (customer == null)
    {
      return null;
    }
    var result = _passwordHasher.VerifyHashedPassword(customer, customer.Password, loginUserDto.Password);
    loginUserDto.UserId = customer.CustomerId;
    loginUserDto.IsAdmin = false;
    return result == PasswordVerificationResult.Success ? loginUserDto : null;

  }


  public async Task<Customer> UpdateCustomerService(Guid customerId, Customer updateCustomer)
  {
    var existingCustomer = await _dbContext.Customers.FindAsync(customerId);
    if (existingCustomer != null)
    {
      existingCustomer.FirstName = updateCustomer.FirstName ?? existingCustomer.FirstName;
      existingCustomer.LastName = updateCustomer.LastName ?? existingCustomer.LastName;
      existingCustomer.Email = updateCustomer.Email ?? existingCustomer.Email;
      existingCustomer.Password = updateCustomer.Password != null ? _passwordHasher.HashPassword(updateCustomer, updateCustomer.Password) : existingCustomer.Password;
      existingCustomer.Mobile = updateCustomer.Mobile ?? existingCustomer.Mobile;
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