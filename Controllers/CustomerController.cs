using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using api.Authentication.Service;
using api.Authentication.Dtos;

namespace api.Controllers;


[ApiController]
[Route("/api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _dbContext;
    private readonly AuthService _authService;

    public CustomerController(CustomerService customerService, AuthService authService)
    {
        _dbContext = customerService;
        _authService = authService;

    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
       
            var customers = await _dbContext.GetAllCustomersService(currentPage , pageSize);
            if (customers.TotalCount < 1)
            {
                return ApiResponse.NotFound("No Customers To Display");

            }
            return ApiResponse.Success<IEnumerable<Customer>>(
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
            var customer = await _dbContext.GetCustomerById(customerIdGuid);
            if (customer == null)
            {
                return ApiResponse.NotFound(
                 $"No Customer Found With ID : ({customerIdGuid})");
            }
            else
            {
                return ApiResponse.Success<Customer>(
                  customer,
                  "Customer is returned successfully"
                );
            }
        
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> CreateCustomer(Customer newCustomer)
    {
        
            var createdCustomer = await _dbContext.CreateCustomerService(newCustomer);
            if (createdCustomer != null)
            {
                return ApiResponse.Created<Customer>(createdCustomer, "Customer is created successfully");
            }
            else
            {
                return ApiResponse.ServerError("Error when creating new customer");

            }
        
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginUserDto loginUserDto)
    {
       
            var LoggedCustomer = await _dbContext.LoginCustomerService(loginUserDto);
            if (LoggedCustomer == null)
            {
                return ApiResponse.UnAuthorized("Invalid Credential");
            }
            var token = _authService.GenerateJwtToken(LoggedCustomer);
            return ApiResponse.Success<LoginUserDto>(LoggedCustomer, "Customer is loggedIn successfully", null, token);


        
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(string customerId, Customer updateCustomer)
    {
        
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return ApiResponse.BadRequest("Invalid Customer ID Format");
            }
            var customer = await _dbContext.UpdateCustomerService(customerIdGuid, updateCustomer);
            if (customer == null)
            {
                return ApiResponse.NotFound("No Customer Founded To Update");
            }
            return ApiResponse.Success<Customer>(
                customer,
                "Customer Is Updated Successfully"
            );
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
        
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return ApiResponse.BadRequest("Invalid Customer ID Format");
            }
            var result = await _dbContext.DeleteCustomerService(customerIdGuid);
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
        
            var result = await _dbContext.ForgotPasswordService(email);
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

            var result = await _dbContext.ResetPasswordService(resetPasswordDto);

            if (!result)
            {
                return ApiResponse.BadRequest("User with this email not found");

            }
            return ApiResponse.Success("Password reset successfully");
    }
}
