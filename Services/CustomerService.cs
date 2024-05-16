using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Data;
using Backend.Dtos;
using Backend.EmailSetup;
using Backend.Helpers;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class CustomerService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;

    public CustomerService(AppDbContext dbContext, IPasswordHasher<Customer> passwordHasher, IEmailSender emailSender, IMapper mapper)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
        _mapper = mapper;
    }


    public async Task<IEnumerable<CustomerDto>> GetAllCustomersService(int currentPage, int pageSize)
    {
        var customers = await _dbContext.Customers
            .Include(a => a.Addresses)
            .Include(o => o.Orders)
            .ThenInclude(op => op.OrderProducts)
            .Include(r => r.Reviews)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var customerDtos = _mapper.Map<List<Customer>, List<CustomerDto>>(customers);

        return customerDtos;
    }


    public async Task<CustomerDto?> GetCustomerById(Guid customerId)
    {
        var customer = await _dbContext.Customers
            .Include(a => a.Addresses)
            .Include(o => o.Orders)
            .ThenInclude(op => op.OrderProducts)
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        var customerDto = _mapper.Map<CustomerDto>(customer);
        return customerDto;
    }


    public async Task<Customer> CreateCustomerService(Customer newCustomer)
    {
        bool userExist = await IsEmailExists(newCustomer.Email);
        if (userExist)
        {
            return (Customer)Results.BadRequest();
        }

        newCustomer.CustomerId = Guid.NewGuid();
        newCustomer.Email = newCustomer.Email.ToLower();
        newCustomer.CreatedAt = DateTime.UtcNow;
        newCustomer.Password = _passwordHasher.HashPassword(newCustomer, newCustomer.Password);
        _dbContext.Customers.Add(newCustomer);
        await _dbContext.SaveChangesAsync();
        return newCustomer;
    }


    public async Task<LoginUserDto?> LoginCustomerService(LoginUserDto loginUserDto)
    {
        var customer = await _dbContext.Customers.SingleOrDefaultAsync(c => c.Email == loginUserDto.Email.ToLower());
        if (customer == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(customer, customer.Password, loginUserDto.Password);
        loginUserDto.UserId = customer.CustomerId;
        loginUserDto.IsAdmin = false;
        return result == PasswordVerificationResult.Success ? loginUserDto : null;
    }


    public async Task<Customer?> UpdateCustomerService(Guid customerId, CustomerDto updateCustomer)
    {
        var existingCustomer = await _dbContext.Customers.FindAsync(customerId);
        if (existingCustomer != null)
        {
            existingCustomer.FirstName = updateCustomer.FirstName.IsNullOrEmpty() ? existingCustomer.FirstName : updateCustomer.FirstName;
            existingCustomer.LastName = updateCustomer.LastName.IsNullOrEmpty() ? existingCustomer.LastName : updateCustomer.LastName;
            existingCustomer.Email = updateCustomer.Email.IsNullOrEmpty() ? existingCustomer.Email : updateCustomer.Email.ToLower();
            existingCustomer.Mobile = updateCustomer.Mobile.IsNullOrEmpty() ? existingCustomer.Mobile : updateCustomer.Mobile;
            existingCustomer.Image = updateCustomer.Image.IsNullOrEmpty() ? existingCustomer.Image : updateCustomer.Image;
            existingCustomer.IsBanned = updateCustomer.IsBanned == existingCustomer.IsBanned ? existingCustomer.IsBanned : updateCustomer.IsBanned;
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


    public async Task<bool> ForgotPasswordService(string email)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(e => e.Email == email);
        if (customer == null)
        {
            return false;
        }

        var resetToken = Guid.NewGuid();
        customer.ResetToken = resetToken;
        customer.ResetTokenExpiration = DateTime.UtcNow.AddHours(1);
        // bc we still not have real host so i will just send a token so we can test it using swagger in the production adjust this 2 lines
        // string resetLink = $"http://localhost:5125/api/admins/reset-password?email={email}&token={resetToken}";

        await _emailSender.SendEmailAsync(email, "Password Reset", $"Dear {customer.FirstName},\nThis is your token {resetToken} to reset your password");
        await _dbContext.SaveChangesAsync();
        return true;
    }


    public async Task<bool> ResetPasswordService(ResetPasswordDto resetPasswordDto)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
        if (customer == null || customer.ResetToken != resetPasswordDto.Token || customer.ResetTokenExpiration < DateTime.UtcNow)
        {
            return false;
        }

        customer.Password = _passwordHasher.HashPassword(customer, resetPasswordDto.NewPassword);
        customer.ResetToken = null;
        customer.ResetTokenExpiration = null;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await _dbContext.Admins.AnyAsync(a => a.Email == email.ToLower()) || await _dbContext.Customers.AnyAsync(c => c.Email == email.ToLower());
    }

    public async Task<int> GetTotalCustomerCount()
    {
        return await _dbContext.Customers.CountAsync();
    }
}