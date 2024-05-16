using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Backend.Data;
using Backend.Dtos;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using SendGrid.Helpers.Errors.Model;

namespace Backend.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly AuthService _authService;

    public CustomerController(CustomerService customerService, AuthService authService)
    {
        _customerService = customerService;
        _authService = authService;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var customers = await _customerService.GetAllCustomersService(currentPage, pageSize);
        int totalCount = await _customerService.GetTotalCustomerCount();

        if (totalCount < 1)
        {
            throw new NotFoundException("No Customers To Display");
        }

        return ApiResponse.Success(
            customers,
            "Customers are returned successfully", new PaginationMeta
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount
            });
    }


    [Authorize]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer(string customerId)
    {
        if (!Guid.TryParse(customerId, out Guid customerIdGuid))
        {
            throw new ValidationException("Invalid customer ID Format");
        }

        var customer = await _customerService.GetCustomerById(customerIdGuid) ?? throw new NotFoundException($"No Customer Found With ID : ({customerIdGuid})");

        return ApiResponse.Success(customer,
            "Customer is returned successfully");
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> CreateCustomer(Customer newCustomer)
    {
        var createdCustomer = await _customerService.CreateCustomerService(newCustomer) ?? throw new Exception("Email already exists");

        return ApiResponse.Created(createdCustomer, "Customer is created successfully");
    }

    private bool IsAddressNamesUnique(Customer customer)
    {
        var addressNames = customer.Addresses.Select(a => a.Name);
        return addressNames.Distinct().Count() == addressNames.Count();
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginCustomer([FromBody] LoginUserDto loginUserDto)
    {
        var LoggedCustomer = await _customerService.LoginCustomerService(loginUserDto) ?? throw new UnauthorizedException("Invalid Email or Password");

        var token = _authService.GenerateJwtToken(LoggedCustomer);
        return ApiResponse.Success(LoggedCustomer, "Customer is loggedIn successfully", null, token);
    }


    [Authorize]
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(string customerId, CustomerDto updateCustomer)
    {
        if (!Guid.TryParse(customerId, out Guid customerIdGuid))
        {
            throw new ValidationException("Invalid Customer ID Format");
        }

        var customer = await _customerService.UpdateCustomerService(customerIdGuid, updateCustomer) ?? throw new NotFoundException("No Customer Founded To Update");

        return ApiResponse.Success(
            customer,
            "Customer Is Updated Successfully"
        );
    }


    [Authorize]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
        if (!Guid.TryParse(customerId, out Guid customerIdGuid))
        {
            throw new ValidationException("Invalid Customer ID Format");
        }

        var result = await _customerService.DeleteCustomerService(customerIdGuid);
        if (!result)
        {
            throw new NotFoundException("The Customer is not found to be deleted");
        }

        return ApiResponse.Success(" Customer is deleted successfully");
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var result = await _customerService.ForgotPasswordService(email);
        if (!result)
        {
            throw new NotFoundException("No user found with this email");
        }

        return ApiResponse.Success("Password reset email sent successfully");
    }


    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
        {
            throw new NotFoundException("Passwords do not match");
        }

        var result = await _customerService.ResetPasswordService(resetPasswordDto);
        if (!result)
        {
            throw new NotFoundException("User with this email not found");
        }

        return ApiResponse.Success("Password reset successfully");
    }
}