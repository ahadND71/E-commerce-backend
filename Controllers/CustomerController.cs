using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Backend.Data;
using Backend.Dtos;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;

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


    // [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var customers = await _customerService.GetAllCustomersService(currentPage, pageSize);
        if (customers.TotalCount < 1)
        {
            return ApiResponse.NotFound("No Customers To Display");
        }
        return ApiResponse.Success(
            customers.Items,
           "Customers are returned successfully");
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer(string customerId)
    {
        if (!Guid.TryParse(customerId, out Guid customerIdGuid))
        {
            return ApiResponse.BadRequest("Invalid customer ID Format");
        }

        var customer = await _customerService.GetCustomerById(customerIdGuid);
        if (customer == null)
        {
            return ApiResponse.NotFound(
             $"No Customer Found With ID : ({customerIdGuid})");
        }
        else
        {
            return ApiResponse.Success(customer,
           "Customer is returned successfully");
        }
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> CreateCustomer(Customer newCustomer)
    {
        var createdCustomer = await _customerService.CreateCustomerService(newCustomer);
        if (createdCustomer != null)
        {
            return ApiResponse.Created(createdCustomer, "Customer is created successfully");
        }
        else
        {
            return ApiResponse.ServerError("Email already exists");
        }
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginCustomer([FromBody] LoginUserDto loginUserDto)
    {
        var LoggedCustomer = await _customerService.LoginCustomerService(loginUserDto);
        if (LoggedCustomer == null)
        {
            return ApiResponse.UnAuthorized("Invalid Credential");
        }

        var token = _authService.GenerateJwtToken(LoggedCustomer);
        return ApiResponse.Success(LoggedCustomer, "Customer is loggedIn successfully", null, token);
    }


    [Authorize]
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(string customerId, CustomerDto updateCustomer)
    {
        if (!Guid.TryParse(customerId, out Guid customerIdGuid))
        {
            return ApiResponse.BadRequest("Invalid Customer ID Format");
        }

        var customer = await _customerService.UpdateCustomerService(customerIdGuid, updateCustomer);
        if (customer == null)
        {
            return ApiResponse.NotFound("No Customer Founded To Update");
        }
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
            return ApiResponse.BadRequest("Invalid Customer ID Format");
        }

        var result = await _customerService.DeleteCustomerService(customerIdGuid);
        if (!result)
        {
            return ApiResponse.NotFound("The Customer is not found to be deleted");
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
            return ApiResponse.NotFound("No user found with this email");
        }

        return ApiResponse.Success("Password reset email sent successfully");
    }


    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
        {
            return ApiResponse.BadRequest("Passwords do not match");
        }

        var result = await _customerService.ResetPasswordService(resetPasswordDto);
        if (!result)
        {
            return ApiResponse.BadRequest("User with this email not found");
        }

        return ApiResponse.Success("Password reset successfully");
    }
}
