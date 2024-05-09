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
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var customers = await _dbContext.GetAllCustomersService();
            if (customers.ToList().Count < 1)
            {
                return ApiResponse.NotFound("No Customers To Display");

            }
            return ApiResponse.Success<IEnumerable<Customer>>(
                customers,
               "Customers are returned successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Customer list");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer(string customerId)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Customer");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> CreateCustomer(Customer newCustomer)
    {
        try
        {
            var createdCustomer = await _dbContext.CreateCustomerService(newCustomer);
            if (createdCustomer != null)
            {
                return ApiResponse.Created<Customer>(createdCustomer, "Customer is created successfully");
            }
            else
            {
                return ApiResponse.ServerError("Email already exists");

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Customer");
            return ApiResponse.ServerError(ex.Message);

        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginUserDto loginUserDto)
    {
        try
        {
            var LoggedCustomer = await _dbContext.LoginCustomerService(loginUserDto);
            if (LoggedCustomer == null)
            {
                return ApiResponse.UnAuthorized("Invalid Credential");
            }
            var token = _authService.GenerateJwtToken(LoggedCustomer);
            return ApiResponse.Success<LoginUserDto>(LoggedCustomer, "Customer is loggedIn successfully", null, token);


        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot login");
            return ApiResponse.ServerError(ex.Message);
        }
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(string customerId, Customer updateCustomer)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot update the Customer ");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, the Customer can not deleted");
            return ApiResponse.ServerError(ex.Message);

        }
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            var result = await _dbContext.ForgotPasswordService(email);
            if (!result)
            {
                return ApiResponse.NotFound("No user found with this email");
            }
            return ApiResponse.Success("Password reset email sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot send password reset link: {ex.Message}");
            return ApiResponse.ServerError(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot reset password: {ex.Message}");
            return ApiResponse.ServerError(ex.Message);
        }
    }
}
